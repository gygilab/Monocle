
using Monocle.Data;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Microsoft.Data.Sqlite;

namespace Monocle.File {

    public class MzDBWriter : IScanWriter
    {
        public const int HAS_MZ_FLOAT = 1;
        public const int HAS_MZ_DOUBLE = 2;
        public const int HAS_INTENSITY = 4;
        public const int HAS_BASELINE = 8;
        public const int HAS_NOISE = 16;
        public const int HAS_FLAGS = 32;
        public const int HAS_RESOLUTION = 64;

        private SqliteConnection db;
        private SqliteCommand scanInsert;
        private SqliteCommand precursorInsert;
        private SqliteCommand peakInsert;
        private SqliteTransaction loadTransaction;

        private int scanIndex = 0;

        private int peakIndex = 0;

        private int precursorIndex = 0;

        /// <summary>
        /// Creates the SQLite db and tables.
        /// </summary>
        /// <param name="path">Path to the SQLite db file</param>
        public void Open(string path)
        {
            db = new SqliteConnection($"Data Source={path}");
            db.Open();

            string sql = "CREATE TABLE metadata (name VARCHAR(1024), value TEXT)";
            new SqliteCommand(sql, db).ExecuteNonQuery();

            sql = @"CREATE TABLE scans (
                id INT PRIMARY KEY,
                scan INT,
                last_scan INT,
                time DOUBLE,
                precursor_mh DOUBLE,
                precursor_mz DOUBLE,
                charge INT,
                total_intensity DOUBLE,
                polarity VARCHAR(1),
                base_peak_mz DOUBLE,
                base_peak_intensity DOUBLE,
                start_mz DOUBLE,
                end_mz DOUBLE,
                low_mz DOUBLE,
                high_mz DOUBLE,
                parent_scan INT,
                filter_line VARCHAR(512),
                detector_type VARCHAR(12),
                activation_type VARCHAR(12),
                activation_energy DOUBLE,
                ms_level INT,
                scan_type VARCHAR(12),
                parent_type VARCHAR(12),
                master_index INT,
                scan_event INT,
                peak_count INT,
                ion_injection_time DOUBLE,
                elapsed_scan_time DOUBLE,
                cv DOUBLE
            )";
            new SqliteCommand(sql, db).ExecuteNonQuery();

            sql = @"CREATE TABLE scan_peaks (
                id INT PRIMARY KEY,
                scan INT,
                peak_count INT,
                data_type INT,
                data BLOB
            )";
            new SqliteCommand(sql, db).ExecuteNonQuery();

            sql = @"
            CREATE TABLE scan_precursors (
                id INT PRIMARY KEY,
                scan INT,
                precursor_mz DOUBLE,
                precursor_mh DOUBLE,
                precursor_charge INT,
                original_mz DOUBLE,
                original_charge INT,
                isolation_mz DOUBLE,
                isolation_width DOUBLE,
                isolation_specificity DOUBLE,
                precursor_intensity DOUBLE);";
            new SqliteCommand(sql, db).ExecuteNonQuery();

            sql = @"INSERT INTO scans (id, scan, last_scan, time, precursor_mh, precursor_mz, charge, 
                total_intensity, polarity, base_peak_mz, base_peak_intensity, start_mz, end_mz,
                low_mz, high_mz, parent_scan, filter_line, detector_type, activation_type,
                activation_energy, ms_level, scan_type, parent_type, master_index, scan_event,
                peak_count, ion_injection_time, elapsed_scan_time, cv) VALUES
                ($id, $scan, $last_scan, $time, $precursor_mh, $precursor_mz, $charge,
                $total_intensity, $polarity, $base_peak_mz, $base_peak_intensity,
                $start_mz, $end_mz, $low_mz, $high_mz, $parent_scan,
                $filter_line, $detector_type, $activation_type, $activation_energy, $ms_level, $scan_type,
                $parent_type, $master_index, $scan_event, $peak_count, $ion_injection_time, $elapsed_scan_time, $cv)";
            scanInsert = new SqliteCommand(sql, db);

            sql = @"INSERT INTO scan_peaks (id, scan, peak_count, data_type, data)
                VALUES ($id, $scan, $peak_count, $data_type, $data)";
            peakInsert = new SqliteCommand(sql, db);

            sql = @"INSERT INTO scan_precursors
                (id, scan, precursor_mz, precursor_mh, precursor_charge, original_mz, 
                original_charge, isolation_mz, isolation_width, isolation_specificity,
                precursor_intensity) VALUES
                ($id, $scan, $precursor_mz, $precursor_mh, $precursor_charge, $original_mz,
                $original_charge, $isolation_mz, $isolation_width, $isolation_specificity,
                $precursor_intensity)";
            precursorInsert = new SqliteCommand(sql, db);

            loadTransaction = db.BeginTransaction();

            scanInsert.Transaction = loadTransaction;
            peakInsert.Transaction = loadTransaction;
            precursorInsert.Transaction = loadTransaction;
        }

        /// <summary>
        /// Writes the header for the file.
        /// For mzdb, no information from the header is saved.
        /// </summary>
        public void WriteHeader(ScanFileHeader header)
        {
            string sql = "INSERT INTO metadata (name, value) VALUES ($name, $value)";
            var command = new SqliteCommand(sql, db);
            command.Transaction = loadTransaction;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("$name", "version");
            command.Parameters.AddWithValue("$value", "1");
            command.ExecuteNonQuery();

            command.Parameters.Clear();
            command.Parameters.AddWithValue("$name", "compression");
            command.Parameters.AddWithValue("$value", "none");
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Closes the connection to the sqlite DB.
        /// Creates indexes before closing.
        /// </summary>
        public void Close()
        {
            if (db == null) {
                return;
            }

            loadTransaction.Commit();

            string sql = "CREATE UNIQUE INDEX scan_index on scans (scan)";
            new SqliteCommand(sql, db).ExecuteNonQuery();

            sql = "CREATE UNIQUE INDEX scan_peak_index on scan_peaks (scan)";
            new SqliteCommand(sql, db).ExecuteNonQuery();

            sql = "CREATE INDEX scan_precursors_index on scan_precursors (scan)";
            new SqliteCommand(sql, db).ExecuteNonQuery();

            db.Close();
        }

        /// <summary>
        /// Writes a scan to the mzdb file.
        /// </summary>
        public void WriteScan(Scan scan)
        {
            double precursorMH = 0;
            double precursorMz = 0;
            int charge = 0;
            if (scan.Precursors.Count > 0) {
                precursorMH = scan.Precursors[0].Mh;
                precursorMz = scan.Precursors[0].Mz;
                charge = scan.Precursors[0].Charge;
            }
            scanInsert.Parameters.Clear();
            scanInsert.Parameters.AddWithValue("$id", ++scanIndex);
            scanInsert.Parameters.AddWithValue("$scan", scan.ScanNumber);
            // No support for merged scans yet.
            scanInsert.Parameters.AddWithValue("$last_scan", scan.ScanNumber);
            // RT in min
            scanInsert.Parameters.AddWithValue("$time", scan.RetentionTime);
            scanInsert.Parameters.AddWithValue("$precursor_mh", precursorMH);
            scanInsert.Parameters.AddWithValue("$precursor_mz", precursorMz);
            scanInsert.Parameters.AddWithValue("$charge", charge);
            scanInsert.Parameters.AddWithValue("$total_intensity", scan.TotalIonCurrent);
            scanInsert.Parameters.AddWithValue("$polarity", scan.Polarity == Polarity.Negative ? "-" : "+");
            scanInsert.Parameters.AddWithValue("$base_peak_mz", scan.BasePeakMz);
            scanInsert.Parameters.AddWithValue("$base_peak_intensity", scan.BasePeakIntensity);
            scanInsert.Parameters.AddWithValue("$start_mz", scan.StartMz);
            scanInsert.Parameters.AddWithValue("$end_mz", scan.EndMz);
            scanInsert.Parameters.AddWithValue("$low_mz", scan.LowestMz);
            scanInsert.Parameters.AddWithValue("$high_mz", scan.HighestMz);
            scanInsert.Parameters.AddWithValue("$parent_scan", scan.PrecursorMasterScanNumber);
            scanInsert.Parameters.AddWithValue("$filter_line", scan.FilterLine);
            scanInsert.Parameters.AddWithValue("$detector_type", scan.DetectorType);
            scanInsert.Parameters.AddWithValue("$activation_type", scan.PrecursorActivationMethod);
            scanInsert.Parameters.AddWithValue("$activation_energy", scan.CollisionEnergy);
            scanInsert.Parameters.AddWithValue("$ms_level", scan.MsOrder);
            scanInsert.Parameters.AddWithValue("$scan_type", scan.ScanType);
            scanInsert.Parameters.AddWithValue("$parent_type", "");
            scanInsert.Parameters.AddWithValue("$master_index", scan.MasterIndex);
            scanInsert.Parameters.AddWithValue("$scan_event", scan.ScanEvent);
            scanInsert.Parameters.AddWithValue("$peak_count", scan.Centroids.Count);
            scanInsert.Parameters.AddWithValue("$ion_injection_time", scan.IonInjectionTime);
            scanInsert.Parameters.AddWithValue("$elapsed_scan_time", scan.ElapsedScanTime);
            scanInsert.Parameters.AddWithValue("$cv", scan.FaimsCV);
            scanInsert.ExecuteNonQuery();

            peakInsert.Parameters.Clear();
            peakInsert.Parameters.AddWithValue("$id", ++peakIndex);
            peakInsert.Parameters.AddWithValue("$scan", scan.ScanNumber);
            peakInsert.Parameters.AddWithValue("$peak_count", scan.Centroids.Count);
            peakInsert.Parameters.AddWithValue("$data_type", getPeakFlags(scan));
            // skipping compression for now.
            peakInsert.Parameters.AddWithValue("$data", encodePeaks(scan));
            peakInsert.ExecuteNonQuery();

            foreach(Precursor precursor in scan.Precursors) {
                precursorInsert.Parameters.Clear();
                precursorInsert.Parameters.AddWithValue("$id", ++precursorIndex);
                precursorInsert.Parameters.AddWithValue("$scan", scan.ScanNumber);
                precursorInsert.Parameters.AddWithValue("$precursor_mz", precursor.Mz);
                precursorInsert.Parameters.AddWithValue("$precursor_mh", precursor.Mh);
                precursorInsert.Parameters.AddWithValue("$precursor_charge", precursor.Charge);
                precursorInsert.Parameters.AddWithValue("$original_mz", precursor.Mz);
                precursorInsert.Parameters.AddWithValue("$original_charge", precursor.Charge);
                precursorInsert.Parameters.AddWithValue("$isolation_mz", precursor.IsolationMz);
                precursorInsert.Parameters.AddWithValue("$isolation_width", precursor.IsolationWidth);
                precursorInsert.Parameters.AddWithValue("$isolation_specificity", precursor.IsolationSpecificity);
                precursorInsert.Parameters.AddWithValue("$precursor_intensity", precursor.Intensity);
                precursorInsert.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Returns an int with the flags to use with the peak data in this sacn.
        /// </summary>
        /// <param name="scan">The scan with the peak data.</param>
        /// <returns>An integer with the flags for the type of data stored.</returns>
        private int getPeakFlags(Scan scan) {
            int output = 0;
            if (scan.DetectorType == "FTMS" || scan.DetectorType == "ASTMS") {
                output = HAS_MZ_DOUBLE | HAS_INTENSITY | HAS_BASELINE | HAS_NOISE;
            }
            else {
                output = HAS_MZ_FLOAT | HAS_INTENSITY;
            }
            if (scan.DetectorType == "ASTMS") {
                output |= HAS_RESOLUTION;
            }
            return output;
        }

        /// <summary>
        /// Encodes the peaks into a byte array according to the input peak flags.
        /// </summary>
        /// <param name="scan">The scan with the peaks to store</param>
        /// <returns>A byte array with the peak data.</returns>
        private byte[] encodePeaks(Scan scan) {
            int peakCount = scan.Centroids.Count;
            double[] mz = new double[peakCount];
            float[] all = new float[peakCount * 5];
            uint[] resolution = new uint[peakCount];
            for (int i = 0; i < scan.Centroids.Count; ++i) {
                Centroid peak = scan.Centroids[i];
                mz[i] = peak.Mz;
                all[i] = (float) peak.Mz;
                all[i + peakCount] = (float) peak.Intensity;
                all[i + (peakCount * 2)] = (float) peak.Baseline;
                all[i + (peakCount * 3)] = (float) peak.Noise;
                resolution[i] = peak.Resolution;
            }

            int mzFloatBytes = peakCount * sizeof(float);
            int mzDoubleBytes = peakCount * sizeof(double);
            int intensityBytes = peakCount * sizeof(float);
            int baselineBytes = peakCount * sizeof(float);
            int noiseBytes = peakCount * sizeof(float);
            int resolutionBytes = peakCount * sizeof(uint);

            byte[] output = null;
            if (scan.DetectorType == "FTMS") {
                output = new byte[mzDoubleBytes + intensityBytes + baselineBytes + noiseBytes];
                Buffer.BlockCopy(mz, 0, output, 0, mzDoubleBytes);
                Buffer.BlockCopy(all, mzFloatBytes, output, mzDoubleBytes, intensityBytes + baselineBytes + noiseBytes);
            }
            else if (scan.DetectorType == "ASTMS") {
                output = new byte[mzDoubleBytes + intensityBytes + baselineBytes + noiseBytes + resolutionBytes];
                Buffer.BlockCopy(mz, 0, output, 0, mzDoubleBytes);
                Buffer.BlockCopy(all, mzFloatBytes, output, mzDoubleBytes, intensityBytes + baselineBytes + noiseBytes);
                Buffer.BlockCopy(resolution, 0, output, mzDoubleBytes + intensityBytes + baselineBytes + noiseBytes, resolutionBytes);
            }
            else {
                output = new byte[mzFloatBytes + intensityBytes];
                Buffer.BlockCopy(all, 0, output, 0, mzFloatBytes + intensityBytes);
            }

            return output;
        }

        /// <summary>
        /// Uses zlib compression to reduce data size.
        /// </summary>
        /// <param name="data">input bytes</param>
        /// <returns>The compresed bytes</returns>
        public static byte[] CompressData(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (var compressed = new DeflaterOutputStream(stream))
                {
                    compressed.Write(data, 0, data.Length);
                }
                return stream.ToArray();
            }
        }
    }

}

