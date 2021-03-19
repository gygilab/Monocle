using System;
using System.IO;

namespace Monocle.File
{
    public class ScanReaderFactory
    {
        /// <summary>
        /// Choose the file reader to read the current data file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IScanReader GetReader(string path)
        {
            string extension = Path.GetExtension(path).ToLower();
            if (extension == ".mzxml")
            {
                return new MzXmlReader();
            }
            else if (extension == ".raw")
            {
                return new RawReader();
            }
            else if (extension == ".mzdb")
            {
                return new MzDBReader();
            }
            else if (extension == ".mzml")
            {
                return new MzMlReader();
            }
            throw new ArgumentException("Unrecognized file extension: " + extension);
        }
    }
}
