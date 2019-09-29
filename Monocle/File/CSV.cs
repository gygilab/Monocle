using Monocle.Data;
using System.Collections.Generic;
using System.IO;

namespace Monocle.File
{
    public class CSV
    {
        /// <summary>
        /// Write a new mzXML file
        /// </summary>
        /// <param name="scans"></param>
        public static void Write(string csvFilePath, List<Scan> scans)
        {
            if (csvFilePath == "")
            {
                throw new IOException("Output CSV path is invalid.");
            }

            string fileName = Path.GetFileNameWithoutExtension(csvFilePath);
            string path = Path.GetDirectoryName(csvFilePath)
                + Path.DirectorySeparatorChar
                + Path.GetFileNameWithoutExtension(csvFilePath)
                + "_monocle.csv";

            var writer = new StreamWriter(System.IO.File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
            writer.AutoFlush = true;
            writer.WriteLine(FlatScanExtension.CsvHeaderString());
            foreach (Scan scan in scans)
            {
                writer.WriteLine(scan.ScanToMonocleString());
            }
        }
    }
}
