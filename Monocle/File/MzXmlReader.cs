using Monocle.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Monocle.File
{
    public class MzXmlReader : IScanReader
    {
        private XmlDocument doc;

        #region Attributes for the MZXML file
        public Dictionary<string, string> mzxmlAttributes = new Dictionary<string, string>()
        {
            { "num" , "ScanNumber" },
            { "msLevel" , "MsOrder" },
            { "scanEvent" , "ScanEvent" },
            { "masterIndex" , "MasterIndex" },
            { "peaksCount" , "PeakCount" },
            { "ionInjectionTime" , "IonInjectionTime" },
            { "elapsedScanTime" , "ElapsedScanTime" },
            { "polarity" , "Polarity" },
            { "scanType" , "ScanType" },
            { "filterLine" , "FilterLine" },
            { "retentionTime","RetentionTimeString" },
            { "startMz","StartMz" },
            { "endMz","EndMz" },
            { "lowMz","LowestMz" },
            { "highMz","HighestMz" },
            { "basePeakMz","BasePeakMz" },
            { "basePeakIntensity","BasePeakIntensity" }
        };

        public Dictionary<string, string> mzxmlMsnAttributes = new Dictionary<string, string>()
        {
            { "totIonCurrent","TotalIonCurrent" },
            { "collisionEnergy","CollisionEnergy" }
        };

        public Dictionary<string, string> mzxmlPrecursorAttributes = new Dictionary<string, string>()
        {
            // Precusor information
            { "precursorMz","PrecursorMz" },
            { "precursorScanNum","PrecursorMasterScanNumber" },
            { "precursorIntensity","PrecursorIntensity" },
            { "precursorCharge","PrecursorCharge" },
            { "activationMethod","PrecursorActivationMethod" }
        };

        public Dictionary<string, string> mzxmlPeaksAttributes = new Dictionary<string, string>()
        {
            // Peaks information
            { "precision","PeaksPrecision" },
            { "byteOrder","PeaksByteOrder" },
            { "contentType","PeaksContentType" },
            { "compressionType", "PeaksCompressionType" },
            { "compressedLen", "PeaksCompressedLength" }
        };
        #endregion
        /// <summary>
        /// Open new fileStream to mzXML file.
        /// </summary>
        /// <param name="path"></param>
        public void Open(string path) {
            if (!System.IO.File.Exists(path)) {
                throw new IOException("File not found: " + path);
            }

            doc = new XmlDocument();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                doc.Load(fs);
            }
        }

        /// <summary>
        /// Open the given file and import scans into the reader.
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerator GetEnumerator() {
            using (XmlNodeList scanElems = doc.GetElementsByTagName("scan"))
            {
                foreach (XmlNode node in scanElems)
                {
                    Scan scan = new Scan();
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        SetAttribute(scan, attr.Name, attr.Value);
                    }

                    //Process child nodes
                    XmlNodeList children = node.ChildNodes;
                    foreach (XmlNode child in children)
                    {
                        SetAttribute(scan, child.Name, child.InnerText);
                        foreach (XmlAttribute attr in child.Attributes)
                        {
                            SetAttribute(scan, attr.Name, attr.Value);
                        }

                        if(child.Name == "peaks") {
                            scan.Centroids = ReadPeaks(child.InnerText, scan.PeakCount);
                        }
                    }

                    yield return scan;
                }
            }
        }

        /// <summary>
        /// Check and set attribute based on attributes dictionary
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public void SetAttribute(Scan scan, string attribute, string value)
        {
            string tempAttr = "";
            if (mzxmlAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlAttributes[attribute];
            }
            else if (mzxmlPrecursorAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlPrecursorAttributes[attribute];
            }
            else if (mzxmlPeaksAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlPeaksAttributes[attribute];
            }
            else if (mzxmlMsnAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlMsnAttributes[attribute];
            }

            if (tempAttr != "")
            {
                PropertyInfo piTmp;
                double dTmp; bool bTmp;
                if (typeof(Scan).GetProperty(tempAttr) != null) //check names even though readOnly DGV
                {
                    piTmp = typeof(Scan).GetProperty(tempAttr);

                    if (piTmp.PropertyType == typeof(int) && Int32.TryParse(value, out int iTmp))
                    {
                        piTmp.SetValue(scan, iTmp);
                    }
                    else if (piTmp.PropertyType == typeof(string))
                    {
                        piTmp.SetValue(scan, value);
                    }
                    else if (piTmp.PropertyType == typeof(double) && Double.TryParse(value, out dTmp))
                    {
                        piTmp.SetValue(scan, dTmp);
                    }
                    else if (piTmp.PropertyType == typeof(bool) && Boolean.TryParse(value, out bTmp))
                    {
                        piTmp.SetValue(scan, bTmp);
                    }
                }
            }
        }

        /// <summary>
        /// Check and GET attribute based on attributes dictionary
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public string GetAttribute(string attribute)
        {
            string tempAttr = "";
            if (mzxmlAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlAttributes[attribute];
            }
            else if (mzxmlPrecursorAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlPrecursorAttributes[attribute];
            }
            else if (mzxmlPeaksAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlPeaksAttributes[attribute];
            }
            else if (mzxmlMsnAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlMsnAttributes[attribute];
            }

            if (tempAttr != "") {
                if (typeof(Scan).GetProperty(tempAttr) != null) //check names even though readOnly DGV
                {
                    object output = GetType().GetProperty(tempAttr).GetValue(this, null);
                    if(output.GetType() == typeof(int))
                    {
                        return int.Parse(output.ToString()).ToString();
                    }
                    else if (output.GetType() == typeof(bool))
                    {
                        return bool.Parse(output.ToString()).ToString();
                    }
                    else if (output.GetType() == typeof(double))
                    {
                        return double.Parse(output.ToString()).ToString();
                    }
                    else if (output.GetType() == typeof(string))
                    {
                        return output.ToString();
                    }
                    return "";
                }
            }
            return null;
        }

        /// <summary>
        /// Read mzXML peaks property
        /// </summary>
        /// <param name="str"></param>
        /// <param name="peakCount"></param>
        /// <returns></returns>
        private List<Centroid> ReadPeaks(string str,int peakCount) {
            List<Centroid> peaks = new List<Centroid>();
            int size = peakCount * 2;
            if (String.Compare(str, "AAAAAAAAAAA=") == 0)
            {
                return peaks;
            }
            byte[] byteEncoded = Convert.FromBase64String(str);
            Array.Reverse(byteEncoded);
            float[] values = new float[size];
            for(int i = 0; i < size; i++)
            {
                values[i] = BitConverter.ToSingle(byteEncoded, i * 4);
            }
            Array.Reverse(values);
            for (int i = 0; i < peakCount; ++i)
            {
                Centroid tempCent = new Centroid(values[2 * i], values[(2 * i) + 1]);
                peaks.Add(tempCent);
            }
            return peaks;
        }
   }
}
