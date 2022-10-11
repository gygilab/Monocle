
using System;

namespace Monocle.Data {
    /// <summary>
    /// Stores metadata from scan data files like RAW or mzXML.
    /// </summary>
    public class ScanFileHeader {
        /// <summary>
        /// The number of scans contained in the file.
        /// </summary>
        public long ScanCount { get; set; } = 0;

        /// <summary>
        /// Time of the first scan in minutes.
        /// </summary>
        public double StartTime { get; set; } = 0;

        /// <summary>
        /// Time of the last scan in minutes.
        /// </summary>
        public double EndTime { get; set; } = 0;

        /// <summary>
        /// Name of the source file.
        /// This should not include the directory.
        /// </summary>
        public string FileName { get; set; } = "";

        /// <summary>
        /// Date the file was created.
        /// </summary>
        public DateTime AcquisitionDate { get; set; }

        /// <summary>
        /// Full file path of the source file.
        /// </summary>
        public string FilePath { get; set; } = "";

        /// <summary>
        /// Name of the Manufacturer of the instrument.
        /// </summary>
        public string InstrumentManufacturer { get; set; } = "unknown";

        /// <summary>
        /// Name of the model of the instrument.
        /// </summary>
        public string InstrumentModel { get; set; } = "unknown";

    }
}
