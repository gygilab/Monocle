using Monocle.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Monocle.File
{
    public class CSV : InputFile
    {
        /// <summary>
        /// Write a new mzXML file
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="scans"></param>
        public static void Write(string xmlFilePath, List<Scan> scans)
        {
            if (xmlFilePath == "" || !System.IO.File.Exists(xmlFilePath))
            {
                Debug.WriteLine("No file at that location.");
            }
            MonocleXmlDocument doc = new MonocleXmlDocument()
            {
                //Assume that the mzXML is named for the RAW file...
                ParentFile = Path.GetFileName(ParentFile),
                ParentFileType = "MonocleRAWData"
            };
            doc.BuildInitialMzxml();
            foreach (Scan scan in scans)
            {
                doc.IndexByteCount(scan.ScanNumber);
                doc.ScanToXml(scan);
            }

            string fileName = Path.GetFileNameWithoutExtension(xmlFilePath);

            doc.GetElementsByTagName("indexOffset")[0].InnerText = doc.ByteCount.ToString();
            doc.Save(Files.ExportPath + fileName + "_monocle.mzXML");
            Ms1ScansCentroids = new Scan[12];
            Ms1ScanIndex = 0;
            Debug.WriteLine("Finished writing xml: " + Files.ExportPath + fileName + "_monocle.mzXML");
        }
    }
//    our csv output right now is:
//precursor ID
//scan ID
//scan number
//precursor m/z
//precursor M+H
//precursor charge
//original precursor m/z
//original precursor charge
//isolation m/z
//isolation width
//isolation specificity
//precursor intensity(edited)

//the new monocle can skip precursor ID and scan ID - I can tie that in later.isolation m/z and width might not be available in the input so that can be zero.
}
