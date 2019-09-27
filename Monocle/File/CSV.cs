using Monocle.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Monocle.File
{
    public class CSV
    {
        private static StreamWriter writer = null;

        /// <summary>
        /// Write a new mzXML file
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="scans"></param>
        public static void Write(string csvFilePath, List<Scan> scans)
        {
            if (csvFilePath == "" || !System.IO.File.Exists(csvFilePath))
            {
                Console.WriteLine("No file at that location.");
            }
            string fileName = Path.GetFileNameWithoutExtension(csvFilePath);
            Console.WriteLine("Finished writing xml: " + Files.ExportPath + fileName + "_monocle.csv");
            if (writer == null)
            {
                // Open a writer that will overwrite an existing file of the same name.
                writer = new StreamWriter(System.IO.File.Open(Files.ExportPath + fileName + "_monocle.csv", FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
                writer.AutoFlush = true;
            }
            writer.WriteLine(FlatScanExtension.CsvHeaderString());
            foreach (Scan scan in scans)
            {
                writer.WriteLine(scan.ScanToMonocleString());
            }
        }
    }
}
