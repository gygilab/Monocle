using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace MonocleUI.lib
{
    public class MZXML
    {
        public static void ReadXml(string xmlFilePath, ref List<Scan> scans)
        {
            if (xmlFilePath == "" || !File.Exists(xmlFilePath))
            {
                Debug.WriteLine("No proteins in the input.");
            }
            XmlDocument doc = new XmlDocument();
            using (FileStream fs = new FileStream(xmlFilePath, FileMode.Open))
            {
                doc.Load(fs);
            }
            XmlNodeList scanElems = doc.GetElementsByTagName("scan");
            foreach(XmlNode node in scanElems)
            {
                Scan tScan = new Scan();
                foreach (XmlAttribute attr in node.Attributes)
                {
                    tScan.SetAttributeValue(attr.Name, attr.Value);
                }
                XmlNodeList children = node.ChildNodes;
                foreach (XmlNode child in children)
                {
                    if (child.Name == "peaks")
                    {
                        tScan.SetAttributeValue(child.Name, child.InnerText);
                    }
                    foreach (XmlAttribute attr in child.Attributes)
                    {
                        tScan.SetAttributeValue(attr.Name, attr.Value);
                    }
                }
                scans.Add(tScan);
            }
            int count = scans.Count;
            scanElems = null;
            doc = null;
        }

        /// <summary>
        /// Read mzXML peaks property
        /// </summary>
        /// <param name="str"></param>
        /// <param name="peakCount"></param>
        /// <returns></returns>
        public static List<Centroid> ReadPeaks(string str,int peakCount) {
            int count = peakCount;
            int size = count * 2;
            if (String.Compare(str, "AAAAAAAAAAA=") == 0)
            {
                return null;// No data.
            }
            byte[] byteEncoded = Convert.FromBase64String(str);
            Array.Reverse(byteEncoded);
            float[] values = new float[size];
            for(int i = 0; i < size; i++)
            {
                values[i] = BitConverter.ToSingle(byteEncoded, i * 4);
            }
            Array.Reverse(values);
            List<Centroid> peaks = new List<Centroid>();
            for (int i = 0; i < count; ++i)
            {
                Centroid tempCent = new Centroid(values[2 * i], values[(2 * i) + 1]);
                peaks.Add(tempCent);
            }
            return peaks;
        }

        /// <summary>
        /// Write list of centroids to a 'peaks' string
        /// </summary>
        /// <param name="centroids"></param>
        /// <returns></returns>
        public static string WritePeaks(List<Centroid> centroids)
        {
            int count = centroids.Count();
            int size = count * 2;
            float[] values = new float[size];
            for (int i = 0; i < count; i++)
            {
                values[2 * i] = (float)centroids[i].Mz;
                values[(2 * i) + 1] = (float)centroids[i].Intensity;
            }
            Array.Reverse(values);
            SortedList<int,byte> byteEncoded = new SortedList<int, byte>();
            int counter = 0;
            for (int i = 0; i < size; i++)
            {
                List<byte> tempbytes = BitConverter.GetBytes(values[i]).ToList();
                for(int j = 0; j < 4; j++)
                {
                    counter++;
                    byteEncoded.Add(counter, tempbytes[j]);
                }
            }
            byte[] output = byteEncoded.OrderByDescending(b => b.Key).Select(c => c.Value).ToArray();
            return Convert.ToBase64String(output);
        }
    }
}
