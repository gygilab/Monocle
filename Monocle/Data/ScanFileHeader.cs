
namespace Monocle.Data {
    /// <summary>
    /// Stores metadata from scan data files like RAW or mzXML.
    /// </summary>
    public class ScanFileHeader {
        /// <summary>
        /// The number of scans contained in the file.
        /// </summary>
        public long ScanCount = 0;

        /// <summary>
        /// Time of the first scan in minutes.
        /// </summary>
        public double StartTime = 0;

        /// <summary>
        /// Time of the last scan in minutes.
        /// </summary>
        public double EndTime = 0;

        /// <summary>
        /// Name of the source file.
        /// This should not include the directory.
        /// </summary>
        public string FileName = "";

        /// <summary>
        /// Full file path of the source file.
        /// </summary>
        public string FilePath = "";

        /// <summary>
        /// Name of the Manufacturer of the instrument.
        /// </summary>
        public string InstrumentManufacturer = "unknown";

        /// <summary>
        /// Name of the model of the instrument.
        /// </summary>
        public string InstrumentModel = "unknown";

    }
}