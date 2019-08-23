using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Monocle
{
    public class FileWriter
    {
        public string FilePath { get; set; }

        public Dictionary<string,Scan> OutputDictionary { get; set; }

        public FileWriter()
        {

        }

        public void UpdateFilePath(string newFilePath)
        {
            if (File.Exists(newFilePath))
            {
                FilePath = newFilePath;
                OutputDictionary = new Dictionary<string, Scan>();
            }
            else
            {
                throw new Exception("No file exists at that path.");
            }
        }

        public void Save(InputFileType fileType)
        {
            if (fileType == InputFileType.mzxml)
            {
                XDocument xDoc = new XDocument();
                xDoc.Add(ToMzXml());
                xDoc.Save(FilePath);
            }
            else if(fileType == InputFileType.mzml)
            {
                XDocument xDoc = new XDocument();
                xDoc.Add(ToMzMl());
                xDoc.Save(FilePath);
            }
        }

        public XElement ToMzXml()
        {
            XElement newEl;
            if(OutputDictionary.Count() > 0)
            {
                newEl = new XElement("Scans", OutputDictionary
                    .Select(kv => new XElement("Scan", new XElement("Description", kv.Key), new XElement("CentroidCount", kv.Value.CentroidCount))));
            }
            else
            {
                throw new Exception("No data to save.");
            }

            return newEl;
        }

        public XElement ToMzMl()
        {
            XElement newEl;
            if (OutputDictionary.Count() > 0)
            {
                newEl = new XElement("Scans", OutputDictionary
                    .Select(kv => new XElement("Scan", new XElement("Description", kv.Key), new XElement("CentroidCount", kv.Value.CentroidCount))));
            }
            else
            {
                throw new Exception("No data to save.");
            }

            return newEl;
        }
    }
}
