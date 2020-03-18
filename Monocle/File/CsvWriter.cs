
using Monocle.Data;
using System.IO;

namespace Monocle.File {
    /// <summary>
    /// Writes scan data into a CSV format.
    /// </summary>
    public class CsvWriter : IScanWriter
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
            writer.WriteLine("scan number" + delimiter + "ms level" + delimiter + "precursor m/z" + delimiter +
                "precursor M+H" + delimiter + "precursor charge" + delimiter +
                "original precursor m/z" + delimiter + "original precursor charge" + delimiter +
                "isolation m/z" + delimiter + "isolation width" + delimiter +
                "isolation specificity" + delimiter + "precursor intensity");
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
            foreach (var precursor in scan.Precursors) {
                writer.WriteLine(scan.ScanNumber + delimiter +
                    scan.MsOrder + delimiter +
                    precursor.Mz + delimiter +
                    precursor.Mh + delimiter +
                    precursor.Charge + delimiter +
                    precursor.OriginalMz + delimiter +
                    precursor.OriginalCharge + delimiter +
                    precursor.IsolationMz + delimiter +
                    precursor.IsolationWidth + delimiter +
                    precursor.IsolationSpecificity + delimiter +
                    precursor.Intensity);
            }
        }
    }
}