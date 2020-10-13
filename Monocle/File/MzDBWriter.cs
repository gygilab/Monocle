
using Monocle.Data;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Data.SQLite;

namespace Monocle.File {

    public class MzDBWriter : IScanWriter
    {
        public const int HAS_MZ_FLOAT = 1;
        public const int HAS_MZ_DOUBLE = 2;
        public const int HAS_INTENSITY = 4;
        public const int HAS_BASELINE = 8;
        public const int HAS_NOISE = 16;

        private SQLiteConnection db;

        private SQLiteCommand scanInsert;

        private SQLiteCommand precursorInsert;

        private SQLiteCommand peakInsert;

        private SQLiteTransaction loadTransaction;

        private int scanIndex = 0;

        private int peakIndex = 0;

        private int precursorIndex = 0;

        /// <summary>
        /// Creates the SQLite db and tables.
        /// </summary>
        /// <param name="path">Path to the SQLite db file</param>
        public void Open(string path)
        {
            SQLiteConnection.CreateFile(path);
            db = new SQLiteConnection("Data Source=" + path + ";Version=3");
            db.Open();

            string sql = "CREATE TABLE metadata (name VARCHAR(1024), value TEXT)";
            new SQLiteCommand(sql, db).ExecuteNonQuery();

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
            new SQLiteCommand(sql, db).ExecuteNonQuery();

            sql = @"CREATE TABLE scan_peaks (
                id INT PRIMARY KEY,
                scan INT,
                peak_count INT,
                data_type INT,
                data BLOB
            )";
            new SQLiteCommand(sql, db).ExecuteNonQuery();

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
            new SQLiteCommand(sql, db).ExecuteNonQuery();

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
            scanInsert = new SQLiteCommand(sql, db);

            sql = @"INSERT INTO scan_peaks (id, scan, peak_count, data_type, data)
                VALUES ($id, $scan, $peak_count, $data_type, $data)";
            peakInsert = new SQLiteCommand(sql, db);

            sql = @"INSERT INTO scan_precursors
                (id, scan, precursor_mz, precursor_mh, precursor_charge, original_mz, 
                original_charge, isolation_mz, isolation_width, isolation_specificity,
                precursor_intensity) VALUES
                ($id, $scan, $precursor_mz, $precursor_mh, $precursor_charge, $original_mz,
                $original_charge, $isolation_mz, $isolation_width, $isolation_specificity,
                $precursor_intensity)";
            precursorInsert = new SQLiteCommand(sql, db);

            loadTransaction = db.BeginTransaction();
        }

        /// <summary>
        /// Writes the header for the file.
        /// For mzdb, no information from the header is saved.
        /// </summary>
        public void WriteHeader(ScanFileHeader header)
        {
            string sql = "INSERT INTO metadata (name, value) VALUES ($name, $value)";
            var command = new SQLiteCommand(sql, db);
            command.Parameters.AddWithValue("$name", "version");
            command.Parameters.AddWithValue("$value", "1");
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
            new SQLiteCommand(sql, db).ExecuteNonQuery();

            sql = "CREATE UNIQUE INDEX scan_peak_index on scan_peaks (scan)";
            new SQLiteCommand(sql, db).ExecuteNonQuery();

            sql = "CREATE UNIQUE INDEX scan_precursors_index on scan_precursors (scan)";
            new SQLiteCommand(sql, db).ExecuteNonQuery();

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

            peakInsert.Parameters.AddWithValue("$id", ++peakIndex);
            peakInsert.Parameters.AddWithValue("$scan", scan.ScanNumber);
            peakInsert.Parameters.AddWithValue("$peak_count", scan.Centroids.Count);
            int peakFlags = getPeakFlags(scan);
            peakInsert.Parameters.AddWithValue("$data_type", peakFlags);
            peakInsert.Parameters.AddWithValue("$data", CompressData(encodePeaks(scan, peakFlags)));
            peakInsert.ExecuteNonQuery();

            foreach(Precursor precursor in scan.Precursors) {
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
            if (scan.DetectorType == "FTMS") {
                return HAS_MZ_DOUBLE | HAS_INTENSITY | HAS_BASELINE | HAS_NOISE;
            }
            return HAS_MZ_FLOAT | HAS_INTENSITY;
        }

        /// <summary>
        /// Encodes the peaks into a byte array according to the input peak flags.
        /// </summary>
        /// <param name="scan">The scan with the peaks to store</param>
        /// <param name="peakFlags">Flags indicating how to store the data.</param>
        /// <returns>A byte array with the peak data.</returns>
        private byte[] encodePeaks(Scan scan, int peakFlags) {
            int peakCount = scan.Centroids.Count;
            int size = 0;
            if ((peakFlags & HAS_MZ_FLOAT) != 0) {
                size += peakCount * 4;
            }
            if ((peakFlags & HAS_MZ_DOUBLE) != 0) {
                size += peakCount * 8;
            }
            if ((peakFlags & HAS_INTENSITY) != 0) {
                size += peakCount * 4;
            }
            if ((peakFlags & HAS_BASELINE) != 0) {
                size += peakCount * 4;
            }
            if ((peakFlags & HAS_NOISE) != 0) {
                size += peakCount * 4;
            }

            byte[] output = new byte[size];
            int index = 0;
            if ((peakFlags & HAS_MZ_FLOAT) != 0) {
                foreach(var peak in scan.Centroids) {
                    BitConverter.GetBytes((float)peak.Mz).CopyTo(output, index);
                    index += 4;
                }
            }
            if ((peakFlags & HAS_MZ_DOUBLE) != 0) {
                foreach(var peak in scan.Centroids) {
                    BitConverter.GetBytes((double)peak.Mz).CopyTo(output, index);
                    index += 8;
                }
            }
            if ((peakFlags & HAS_INTENSITY) != 0) {
                foreach(var peak in scan.Centroids) {
                    BitConverter.GetBytes((float)peak.Intensity).CopyTo(output, index);
                    index += 4;
                }
            }
            if ((peakFlags & HAS_BASELINE) != 0) {
                foreach(var peak in scan.Centroids) {
                    BitConverter.GetBytes((float)peak.Baseline).CopyTo(output, index);
                    index += 4;
                }
            }
            if ((peakFlags & HAS_NOISE) != 0) {
                foreach(var peak in scan.Centroids) {
                    BitConverter.GetBytes((float)peak.Noise).CopyTo(output, index);
                    index += 4;
                }
            }
            return output;
        }

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

