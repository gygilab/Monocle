
using System;
using System.IO;

namespace Monocle.File
{
    public class ScanWriterFactory
    {
        /// <summary>
        /// Returns a new instance of the appropriate Scan Reader class.
        /// </summary>
        /// <param name="file">The target file being written.</param>
        /// <param name="type">The type of the target file.</param>
        /// <returns></returns>
        public static IScanWriter GetWriter(OutputFileType type)
        {
            switch (type)
            {
                case OutputFileType.csv:
                    return new CsvWriter();
                case OutputFileType.mzxml:
                    return new MzXmlWriter();
                case OutputFileType.exmzxml:
                    return new ExtendedMzXmlWriter();
                case OutputFileType.mzml:
                    return new MzMlWriter();
                default:
                    break;
            }
            throw new Exception("Unrecognized file type selected for output");
        }

        /// <summary>
        /// This method changes the extension of the input filename, 
        /// based on the selected file type.
        /// </summary>
        /// <param name="filename">the filename to change.</param>
        /// <param name="type">the new file type.</param>
        /// <returns></returns>
        public static string MakeTargetFileName(string filename, OutputFileType type) {
            string ext = "";
            switch (type) {
                case OutputFileType.csv:
                    ext = "csv";
                    break;
                case OutputFileType.exmzxml:
                    // pass thru
                case OutputFileType.mzxml:
                    ext = "mzXML";
                    break;
                case OutputFileType.mzml:
                    ext = "mzML";
                    break;
                default:
                    break;
            }
            return Path.ChangeExtension(filename, ext);
        }
    }
}
