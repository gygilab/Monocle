using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MonocleUI.lib
{
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
                XmlAttribute newAttribute = doc.CreateAttribute(attr.Key);
                newAttribute.Value = scan.CheckAndGetValue(attr.Key);
                scanElement.Attributes.Append(newAttribute);
            }

            foreach (KeyValuePair<string, string> attr in scan.PeaksAttributes)
            {
                if(attr.Key == "peaks")
                {
                    peaksElement.InnerText = scan.CheckAndGetValue(attr.Key);
                }
                else
                {
                    XmlAttribute newAttribute = doc.CreateAttribute(attr.Key);
                    newAttribute.Value = scan.CheckAndGetValue(attr.Key);
                    peaksElement.Attributes.Append(newAttribute);
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
                        XmlAttribute newAttribute = doc.CreateAttribute(attr.Key);
                        newAttribute.Value = scan.CheckAndGetValue(attr.Key);
                        precursorElement.Attributes.Append(newAttribute);
                    }
                }
                scanElement.AppendChild(precursorElement);
            }

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
        public static XmlDocument BuildInitialMzxml(this XmlDocument doc, string parentFile, string parentFileType = "RAWData")
        {
            XmlNode xmlNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlNode);
            XmlElement newMzxmlElement = doc.CreateElement("mzXML");
            XNamespace ns0 = "http://sashimi.sourceforge.net/schema_revision/mzXML_3.1";
            XNamespace ns1 = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace ns2 = "http://sashimi.sourceforge.net/schema_revision/mzXML_3.1 http://sashimi.sourceforge.net/schema_revision/mzXML_3.1/mzXML_idx_3.1.xsd";

            /// Scans will also be added to the msRun node!
            /// add to mzXML node
            XmlElement newMsrunElement = doc.CreateElement("msRun");
            XmlAttribute newAttribute = doc.CreateAttribute("scanCount");
            newAttribute.Value = "0";
            newMsrunElement.Attributes.Append(newAttribute);

            newAttribute = doc.CreateAttribute("startTime");
            newAttribute.Value = "0";
            newMsrunElement.Attributes.Append(newAttribute);

            newAttribute = doc.CreateAttribute("endTime");
            newAttribute.Value = "0";
            newMsrunElement.Attributes.Append(newAttribute);

            // add to msRun
            XmlElement newParentFileElement = doc.CreateElement("parentFile");
            newAttribute = doc.CreateAttribute("fileName");
            newAttribute.Value = parentFile;
            newParentFileElement.Attributes.Append(newAttribute);

            newAttribute = doc.CreateAttribute("fileType");
            newAttribute.Value = "parentFileType";
            newParentFileElement.Attributes.Append(newAttribute);

            XmlElement newMsInstrumentElement = doc.CreateElement("msInstrument");
            // add to instrument
            XmlElement newMsManufacturerElement = doc.CreateElement("msManufacturer");
            newAttribute = doc.CreateAttribute("category");
            newAttribute.Value = "msManufacturer";
            newMsManufacturerElement.Attributes.Append(newAttribute);

            newAttribute = doc.CreateAttribute("value");
            newAttribute.Value = "unknown";
            newMsManufacturerElement.Attributes.Append(newAttribute);
            // add to instrument
            XmlElement newMsModelElement = doc.CreateElement("msModel");
            newAttribute = doc.CreateAttribute("category");
            newAttribute.Value = "msModel";
            newMsModelElement.Attributes.Append(newAttribute);

            newAttribute = doc.CreateAttribute("value");
            newAttribute.Value = "unknown";
            newMsModelElement.Attributes.Append(newAttribute);

            /// Instrument contains: msModel and msManufacturer
            newMsInstrumentElement.AppendChild(newMsManufacturerElement);
            newMsInstrumentElement.AppendChild(newMsModelElement);

            /// msRun contains: parentfile, msInstrument (and scans)
            newMsrunElement.AppendChild(newParentFileElement);
            newMsrunElement.AppendChild(newMsInstrumentElement);

            /// mzXML contains: msRun
            newMzxmlElement.AppendChild(newMsrunElement);
            newMzxmlElement.SetAttribute("xmlns", ns0.NamespaceName);
            newMzxmlElement.SetAttribute("xmlns:xsi", ns1.NamespaceName);
            newMzxmlElement.SetAttribute("xsi:schemaLocation", ns2.NamespaceName);

            doc.AppendChild(newMzxmlElement);
            return doc;
        }
    }
}
