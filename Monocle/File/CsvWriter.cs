
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
            writer.WriteLine("scan number" + delimiter + "precursor m/z" + delimiter +
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
            var precursor = scan.Precursors[0];
            writer.WriteLine(scan.ScanNumber + delimiter + //scan number
                precursor.Mz + delimiter +
                precursor.Mh + delimiter +
                precursor.Charge + delimiter +
                precursor.Mz + delimiter + // Original precursor m/z
                precursor.Charge + delimiter + // Original precursor charge
                0 + delimiter + //isolation m/z
                0 + delimiter + //isolation width
                precursor.IsolationSpecificity + delimiter +
                precursor.Intensity);
        }
    }
}