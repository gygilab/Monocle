using System.IO;

namespace Monocle.File {
    public class ScanReaderFactory {
        public static IScanReader GetReader(string path) {
            if (Path.GetExtension(path).ToLower() == ".mzxml")
            {
                return new MzXmlReader();
            }
            else if (Path.GetExtension(path).ToLower() == ".raw")
            {
                return new RawReader();
            }
            return null;
        }
    }
}
