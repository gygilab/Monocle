
using Monocle.Data;
using System.Globalization;
using System.IO;

namespace Monocle.File {
    /// <summary>
    /// Writes scan data into a CSV format.
    /// </summary>
    public class CsvPeaksWriter : IScanWriter
    {
        /// <summary>
        /// The text stream for output.
        /// </summary>
        private StreamWriter writer;

        /// <summary>
        /// Delimiter used between values.
        /// </summary>
        private string delimiter = ",";

        /// <summary>
        /// Opens the file and writes starts writing the header line.
        /// </summary>
        /// <param name="path">Path to the csv file.</param>
        public void Open(string path)
        {
            writer = new StreamWriter(System.IO.File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
            writer.AutoFlush = true;
            writer.WriteLine("scan number" + delimiter + "ms level" + delimiter + "m/z" + delimiter +
                "intensity" + delimiter + "noise" + delimiter +
                "resolution");
        }

        /// <summary>
        /// Closes the text stream.
        /// </summary>
        public void Close() 
        {
            writer.Close();
        }

        /// <summary>
        /// There is no place to store metadata in the csv file,
        /// so calling this method does nothing.
        /// </summary>
        /// <param name="header"></param>
        public void WriteHeader(ScanFileHeader header) {
            // Nothing to do.
        }

        /// <summary>
        /// Writes scan data to a line at the end of the CSV file.
        /// </summary>
        /// <param name="scan"></param>
        public void WriteScan(Scan scan)
        {
            for (int i = 0; i < scan.Centroids.Count; ++i) {
                writer.WriteLine(scan.ScanNumber + delimiter +
                    scan.MsOrder + delimiter +
                    scan.Centroids[i].Mz.ToString("G17", CultureInfo.InvariantCulture) + delimiter +
                    scan.Centroids[i].Intensity.ToString("G17", CultureInfo.InvariantCulture) + delimiter +
                    scan.Centroids[i].Noise.ToString("G17", CultureInfo.InvariantCulture) + delimiter +
                    scan.Centroids[i].Resolution.ToString("G17", CultureInfo.InvariantCulture));
            }
        }
    }
}

