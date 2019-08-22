using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MonocleUI.lib
{
    public class MonocleXmlDoxument : XmlDocument
    {
        public int currentByteCount { get; set; } = 0;

        public override XmlNode AppendChild(XmlNode newChild)
        {
            currentByteCount += Encoding.Unicode.GetByteCount(newChild.InnerText);
            return base.AppendChild(newChild);
        }
    }

    public static class XmlExtensions
    {
        /// <summary>
        /// Add scans to the msRun node in the given document
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="scan"></param>
        /// <returns></returns>
        public static XmlDocument ScanToXml(this XmlDocument doc, Scan scan)
        {
            XmlElement scanElement = doc.CreateElement("scan");
            XmlElement peaksElement = doc.CreateElement("peaks");

            foreach (KeyValuePair<string,string> attr in scan.Attributes)
            {
                XmlAttribute Attribute = doc.CreateAttribute(attr.Key);
                Attribute.Value = scan.CheckAndGetValue(attr.Key);
                scanElement.Attributes.Append(Attribute);
            }

            foreach (KeyValuePair<string, string> attr in scan.PeaksAttributes)
            {
                if(attr.Key == "peaks")
                {
                    peaksElement.InnerText = scan.CheckAndGetValue(attr.Key);
                }
                else
                {
                    XmlAttribute Attribute = doc.CreateAttribute(attr.Key);
                    Attribute.Value = scan.CheckAndGetValue(attr.Key);
                    peaksElement.Attributes.Append(Attribute);
                }
            }
            scanElement.AppendChild(peaksElement);

            if (scan.MsOrder > 1)
            {
                XmlElement precursorElement = doc.CreateElement("precursorMz");
                foreach (KeyValuePair<string, string> attr in scan.PrecursorAttributes)
                {
                    if (attr.Key == "precursorMz")
                    {
                        precursorElement.InnerText = scan.CheckAndGetValue(attr.Key);
                    }
                    else
                    {
                        XmlAttribute Attribute = doc.CreateAttribute(attr.Key);
                        Attribute.Value = scan.CheckAndGetValue(attr.Key);
                        precursorElement.Attributes.Append(Attribute);
                    }
                }
                scanElement.AppendChild(precursorElement);
            }

            doc.GetElementsByTagName("msRun")[0].AppendChild(scanElement);
            return doc;
        }

        public static XmlDocument ScanNumberToOffset(this XmlDocument doc, int ScanNumber)
        {
            XmlElement offsetElement = doc.CreateElement("offset");
            XmlAttribute Attribute = doc.CreateAttribute("id");
            Attribute.Value = ScanNumber.ToString();
            offsetElement.Attributes.Append(Attribute);
            offsetElement.InnerText = Encoding.Unicode.GetByteCount(doc.InnerXml).ToString();

            doc.GetElementsByTagName("index")[0].AppendChild(offsetElement);

            return doc;
        }

        /// <summary>
        /// Build a basic msXML 3.1 document
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parentFile"></param>
        /// <param name="parentFileType"></param>
        /// <returns></returns>
        public static XmlDocument BuildInitialMzxml(this XmlDocument doc, string parentFile, string parentFileType = "RAWData")
        {
            XmlNode xmlNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(xmlNode);
            XmlElement MzxmlElement = doc.CreateElement("mzXML");
            XNamespace ns0 = "http://sashimi.sourceforge.net/schema_revision/mzXML_3.1";
            XNamespace ns1 = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace ns2 = "http://sashimi.sourceforge.net/schema_revision/mzXML_3.1 http://sashimi.sourceforge.net/schema_revision/mzXML_3.1/mzXML_idx_3.1.xsd";

            /// Scans will also be added to the msRun node!
            /// add to mzXML node
            XmlElement MsrunElement = doc.CreateElement("msRun");
            XmlAttribute Attribute = doc.CreateAttribute("scanCount");
            Attribute.Value = "0";
            MsrunElement.Attributes.Append(Attribute);

            Attribute = doc.CreateAttribute("startTime");
            Attribute.Value = "0";
            MsrunElement.Attributes.Append(Attribute);

            Attribute = doc.CreateAttribute("endTime");
            Attribute.Value = "0";
            MsrunElement.Attributes.Append(Attribute);

            // add to msRun
            XmlElement ParentFileElement = doc.CreateElement("parentFile");
            Attribute = doc.CreateAttribute("fileName");
            Attribute.Value = parentFile;
            ParentFileElement.Attributes.Append(Attribute);

            Attribute = doc.CreateAttribute("fileType");
            Attribute.Value = "parentFileType";
            ParentFileElement.Attributes.Append(Attribute);

            // add to index
            XmlElement indexElement = doc.CreateElement("index");
            XmlElement indexOffsetElement = doc.CreateElement("indexOffset");

            Attribute = doc.CreateAttribute("name");
            Attribute.Value = "scan";
            indexElement.Attributes.Append(Attribute);

            XmlElement MsInstrumentElement = doc.CreateElement("msInstrument");
            // add to instrument
            XmlElement MsManufacturerElement = doc.CreateElement("msManufacturer");
            Attribute = doc.CreateAttribute("category");
            Attribute.Value = "msManufacturer";
            MsManufacturerElement.Attributes.Append(Attribute);

            Attribute = doc.CreateAttribute("value");
            Attribute.Value = "unknown";
            MsManufacturerElement.Attributes.Append(Attribute);
            // add to instrument
            XmlElement MsModelElement = doc.CreateElement("msModel");
            Attribute = doc.CreateAttribute("category");
            Attribute.Value = "msModel";
            MsModelElement.Attributes.Append(Attribute);

            Attribute = doc.CreateAttribute("value");
            Attribute.Value = "unknown";
            MsModelElement.Attributes.Append(Attribute);

            /// Instrument contains: msModel and msManufacturer
            MsInstrumentElement.AppendChild(MsManufacturerElement);
            MsInstrumentElement.AppendChild(MsModelElement);

            /// msRun contains: parentfile, msInstrument (and scans)
            MsrunElement.AppendChild(ParentFileElement);
            MsrunElement.AppendChild(MsInstrumentElement);

            /// mzXML contains: msRun, index, and indexOffset
            MzxmlElement.AppendChild(MsrunElement);
            MzxmlElement.AppendChild(indexElement);
            MzxmlElement.AppendChild(indexOffsetElement);
            MzxmlElement.SetAttribute("xmlns", ns0.NamespaceName);
            MzxmlElement.SetAttribute("xmlns:xsi", ns1.NamespaceName);
            MzxmlElement.SetAttribute("xsi:schemaLocation", ns2.NamespaceName);

            doc.AppendChild(MzxmlElement);
            return doc;
        }
    }
}
