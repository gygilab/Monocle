
using Monocle.Data;

namespace Monocle.File {
    public interface IScanWriter
    {
        /// <summary>
        /// Opens the file or other reasource for the scan writer.
        /// </summary>
        /// <param name="path">Input can be a path or other resource identifier.</param>
        void Open(string path);

        /// <summary>
        /// Closes the resources and performs any cleanup.
        /// Calling this is necessary for proper output.
        /// </summary>
        void Close();

        /// <summary>
        /// Writes scans header information to the file.
        /// This is typically called before writing any scans.
        /// </summary>
        /// <param name="header">Data to be written.</param>
        void WriteHeader(ScanFileHeader header);

        /// <summary>
        /// Writes a single scan to the output.
        /// </summary>
        /// <param name="scan">The scan to write.</param>
        void WriteScan(Scan scan);
    }
}
