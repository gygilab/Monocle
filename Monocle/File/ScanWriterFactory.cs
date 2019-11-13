
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
        public static IScanWriter GetWriter(string file, OutputFileType type)
        {
            file = MakeTargetFileName(file, type);
            switch (Path.GetExtension(file).ToUpper())
            {

                case ".CSV":
                    return new CsvWriter();
                case ".MZXML":
                    return new MzXmlWriter();
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
        private static string MakeTargetFileName(string filename, OutputFileType type) {
            string ext = "";
            switch (type) {
                case OutputFileType.csv:
                    ext = "csv";
                    break;
                case OutputFileType.mzxml:
                    ext = "mzXML";
                    break;
                default:
                    break;
            }
            return Path.ChangeExtension(filename, ext);
        }
    }
}
