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
            XmlElement newScanElement = doc.CreateElement("scan");
            foreach(KeyValuePair<string,string> attr in scan.Attributes)
            {
                if (attr.Value.Contains("Precursor"))
                {

                }
                else if (attr.Value.Contains("Peaks"))
                {

                }
                else
                {
                    XmlAttribute newAttribute = doc.CreateAttribute(attr.Key);
                    newAttribute.Value = scan.CheckAndGetValue(attr.Key);
                    newScanElement.Attributes.Append(newAttribute);
                }
            }
            doc.GetElementsByTagName("msRun")[0].AppendChild(newScanElement);
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
            XmlElement newMzxmlElement = doc.CreateElement("mzXML");
            XNamespace ns1 = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace ns2 = "http://sashimi.sourceforge.net/schema_revision/mzXML_3.1 http://sashimi.sourceforge.net/schema_revision/mzXML_3.1/mzXML_idx_3.1.xsd";

            XmlAttribute newAttribute = doc.CreateAttribute("xsi:schemaLocation", ns2.NamespaceName);
            newMzxmlElement.Attributes.Append(newAttribute);

            /// Scans will also be added to the msRun node!
            /// add to mzXML node
            XmlElement newMsrunElement = doc.CreateElement("msRun");
            newAttribute = doc.CreateAttribute("scanCount");
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

            doc.AppendChild(newMzxmlElement);
            return doc;
        }
    }
}
