using Monocle.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monocle.File
{
    public static class FlatFileExtensions
    {
        public static MonocleXmlDocument ScanToXml(this MonocleXmlDocument doc, Scan scan)
        {
            XmlElement scanElement = doc.CreateElement("scan");
            XmlElement peaksElement = doc.CreateElement("peaks");
            int offsetCount = 0;
            foreach (KeyValuePair<string, string> attr in scan.mzxmlAttributes)
            {
                XmlAttribute Attribute = doc.CreateAttribute(attr.Key);
                Attribute.Value = scan.CheckGetMzxmlValue(attr.Key);
                scanElement.Attributes.Append(Attribute);
            }
            offsetCount += 4;
            if (scan.MsOrder > 1)
            {
                foreach (KeyValuePair<string, string> attr in scan.mzxmlMsnAttributes)
                {
                    XmlAttribute Attribute = doc.CreateAttribute(attr.Key);
                    Attribute.Value = scan.CheckGetMzxmlValue(attr.Key);
                    scanElement.Attributes.Append(Attribute);
                }

                XmlElement precursorElement = doc.CreateElement("precursorMz");
                foreach (KeyValuePair<string, string> attr in scan.mzxmlPrecursorAttributes)
                {
                    if (attr.Key == "precursorMz")
                    {
                        precursorElement.InnerText = scan.CheckGetMzxmlValue(attr.Key);
                    }
                    else
                    {
                        XmlAttribute Attribute = doc.CreateAttribute(attr.Key);
                        Attribute.Value = scan.CheckGetMzxmlValue(attr.Key);
                        precursorElement.Attributes.Append(Attribute);
                    }
                }
                scanElement.AppendChild(precursorElement);
                offsetCount += 3;
            }
            foreach (KeyValuePair<string, string> attr in scan.mzxmlPeaksAttributes)
            {
                if (attr.Key == "peaks")
                {
                    peaksElement.InnerText = scan.CheckGetMzxmlValue(attr.Key);
                }
                else
                {
                    XmlAttribute Attribute = doc.CreateAttribute(attr.Key);
                    Attribute.Value = scan.CheckGetMzxmlValue(attr.Key);
                    peaksElement.Attributes.Append(Attribute);
                }
            }
            offsetCount += 6;
            scanElement.AppendChild(peaksElement);
            string sOuterXml = scanElement.OuterXml;
            XDocument xDoc = XDocument.Parse(sOuterXml);
            sOuterXml = xDoc.ToString();
            offsetCount += Encoding.ASCII.GetByteCount(sOuterXml);
            doc.ByteCount += offsetCount + 1;
            doc.GetElementsByTagName("msRun")[0].AppendChild(scanElement);
            return doc;
        }

        /// <summary>
        /// Build a basic msXML 3.1 document
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parentFile"></param>
        /// <param name="parentFileType"></param>
        /// <returns></returns>
        public static void BuildInitialMzxml(this MonocleXmlDocument doc)
        {
        }
        }
}
