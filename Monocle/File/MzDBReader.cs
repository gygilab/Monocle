
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Monocle.Data;
using System;
using System.Collections;
using Microsoft.Data.Sqlite;
using System.IO;


namespace Monocle.File
{
    public class MzDBReader : IScanReader
    {
        private SqliteConnection db;

        string compression = "";

        public void Open(string path, ScanReaderOptions options)
        {
            db = new SqliteConnection("Data Source=" + path);
            db.Open();

            var metadata = new SqliteCommand("SELECT name, value FROM metadata", db);
            using (var reader = metadata.ExecuteReader())
            while (reader.Read())
            {
                if (((string)reader["name"]) == "compression") {
                    compression = (string) reader["value"];
                }
            }
        }

        public ScanFileHeader GetHeader()
        {
            return new ScanFileHeader();
        }

        public void Close()
        {
            if (db == null)
            {
                return;
            }
            db.Close();
        }

        public IEnumerator GetEnumerator()
        {
            var scanCommand = new SqliteCommand("SELECT * FROM scans AS s LEFT JOIN scan_peaks as p ON p.scan=s.scan", db);
            var precursorCommand = new SqliteCommand("SELECT * FROM scan_precursors WHERE scan=$scan", db);

            using (var scanReader = scanCommand.ExecuteReader())
            {
                while (scanReader.Read())
                {
                    var scan = new Scan{
                        ScanNumber = (int)(long) scanReader["scan"],
                        ScanEvent = (int)(long) scanReader["scan_event"],
                        MsOrder = (int)(long) scanReader["ms_level"],
                        PeakCount = (int)(long) scanReader["peak_count"],
                        MasterIndex = (int)(long) scanReader["master_index"],
                        IonInjectionTime = (double) scanReader["ion_injection_time"],
                        ElapsedScanTime = (double) scanReader["elapsed_scan_time"],
                        Polarity = ReadPolarity((string) scanReader["polarity"]),
                        ScanType = (string) scanReader["scan_type"],
                        DetectorType = (string) scanReader["detector_type"],
                        FilterLine = (string) scanReader["filter_line"],
                        RetentionTime = (double) scanReader["time"],
                        StartMz = (double) scanReader["start_mz"],
                        EndMz = (double) scanReader["end_mz"],
                        LowestMz = (double) scanReader["low_mz"],
                        HighestMz = (double) scanReader["high_mz"],
                        BasePeakMz = (double) scanReader["base_peak_mz"],
                        BasePeakIntensity = (double) scanReader["base_peak_intensity"],
                        FaimsCV = (int)(long) (double) scanReader["cv"],
                        TotalIonCurrent = (double) scanReader["total_intensity"],
                        CollisionEnergy = (double) scanReader["activation_energy"],
                        PrecursorMasterScanNumber = (int)(long) scanReader["parent_scan"],
                        PrecursorActivationMethod = (string) scanReader["activation_type"]
                    };

                    int peakFlags = (int)(long) scanReader["data_type"];
                    if (peakFlags > 0) {
                        byte[] data = (byte[])scanReader["data"];
                        if (compression == "zlib") {
                            data = DecompressData(data);
                        }
                        DecodePeaks(scan, peakFlags, data);
                    }

                    precursorCommand.Parameters.Clear();
                    precursorCommand.Parameters.AddWithValue("$scan", scan.ScanNumber);
                    using (var precursorReader = precursorCommand.ExecuteReader()) {
                        while (precursorReader.Read()) {
                            Precursor precursor = new Precursor() {
                                Mz = (double) precursorReader["precursor_mz"],
                                Intensity = (double) precursorReader["precursor_intensity"],
                                Charge = (int)(long) precursorReader["precursor_charge"],
                                OriginalMz = (double) precursorReader["original_mz"],
                                OriginalCharge = (int)(long) precursorReader["original_charge"],
                                IsolationMz = (double) precursorReader["isolation_mz"],
                                IsolationWidth = (double) precursorReader["isolation_width"],
                                IsolationSpecificity = (double) precursorReader["isolation_specificity"]
                            };
                            scan.Precursors.Add(precursor);
                        }
                    }

                    yield return scan;
                }
            }
        }

        private void DecodePeaks(Scan scan, int flags, byte[] data) {
            int peakCount = scan.PeakCount;
            int mzBytes = 0;
            int intensityOffset = 0;
            int baselineOffset = 0;
            int noiseOffset = 0;
            if ((flags & MzDBWriter.HAS_MZ_FLOAT) != 0) {
                mzBytes = 4;
            }
            if ((flags & MzDBWriter.HAS_MZ_DOUBLE) != 0) {
                mzBytes = 8;
            }
            if ((flags & MzDBWriter.HAS_INTENSITY) != 0) {
                intensityOffset = mzBytes * peakCount;
            }
            if ((flags & MzDBWriter.HAS_INTENSITY) != 0) {
                baselineOffset = intensityOffset + (4 * peakCount);
            }
            if ((flags & MzDBWriter.HAS_INTENSITY) != 0) {
                noiseOffset = baselineOffset + (4 * peakCount);
            }
            for (int i = 0; i < peakCount; ++i) {
                Centroid centroid = new Centroid();
                if ((flags & MzDBWriter.HAS_MZ_FLOAT) != 0) {
                    centroid.Mz = BitConverter.ToSingle(data, i * mzBytes);
                }
                if ((flags & MzDBWriter.HAS_MZ_DOUBLE) != 0) {
                    centroid.Mz = BitConverter.ToDouble(data, i * mzBytes);
                }
                if ((flags & MzDBWriter.HAS_INTENSITY) != 0) {
                    centroid.Intensity = BitConverter.ToSingle(data, intensityOffset + (i * 4));
                }
                if ((flags & MzDBWriter.HAS_BASELINE) != 0) {
                    centroid.Baseline = BitConverter.ToSingle(data, baselineOffset + (i * 4));
                }
                if ((flags & MzDBWriter.HAS_NOISE) != 0) {
                    centroid.Noise = BitConverter.ToSingle(data, noiseOffset + (i * 4));
                }
                scan.Centroids.Add(centroid);
            }

            if ((flags & MzDBWriter.HAS_BASELINE) != 0) {
                scan.HasBaseline = true;
            }
            if ((flags & MzDBWriter.HAS_NOISE) != 0) {
                scan.HasNoise = true;
            }
            if ((flags & MzDBWriter.HAS_RESOLUTION) != 0) {
                scan.HasResolution = true;
            }
        }

        private byte[] DecompressData(byte[] data) {
            using (var input = new MemoryStream(data))
            using (var stream = new InflaterInputStream(input))
            {
                byte[] buffer = new byte[4096];
                using (var output = new MemoryStream())
                {
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = stream.Read(buffer, 0, 4096);
                        if (bytesRead > 0)
                        {
                            output.Write(buffer, 0, bytesRead);
                        }
                    }
                    while (bytesRead > 0);
                    return output.ToArray();
                }
            }
        }

        private Polarity ReadPolarity(string polarity) {
            if (polarity == "+") {
                return Polarity.Positive;
            }
            else if(polarity == "-") {
                return Polarity.Negative;
            }
            return Polarity.None;
        }
    }
}
