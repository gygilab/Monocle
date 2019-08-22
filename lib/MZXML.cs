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
        public static List<Scan> ReadXml(string xmlFilePath)
        {
            if (xmlFilePath == "" || !File.Exists(xmlFilePath))
            {
                Debug.WriteLine("No proteins in the input.");
                return null;
            }
            List<Scan> scans = new List<Scan>();
            Scan tScan = new Scan();
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);
            using (XmlReader reader = XmlReader.Create(xmlFilePath))
            {
                while (reader.Read())
                {
                    tScan = new Scan();
                    if(reader.Name == "scan" && reader.NodeType == XmlNodeType.EndElement)
                    {
                        scans.Add(tScan);
                    }
                    else if (reader.Name == "scan" && reader.NodeType == XmlNodeType.Element)
                    {
                        for (int attInd = 0; attInd < reader.AttributeCount; attInd++)
                        {
                            reader.MoveToAttribute(attInd);
                            tScan.SetAttributeValue(reader.Name, reader.Value);
                        }
                        if(reader.ReadToDescendant("precursorMz")){
                            for (int attInd = 0; attInd < reader.AttributeCount; attInd++)
                            {
                                reader.MoveToAttribute(attInd);
                                tScan.SetAttributeValue(reader.Name, reader.Value);
                            }
                        }
                        else if (reader.ReadToDescendant("peaks"))
                        {
                            tScan.SetAttributeValue(reader.Name, reader.ReadElementContentAsString());
                            for (int attInd = 0; attInd < reader.AttributeCount; attInd++)
                            {
                                reader.MoveToAttribute(attInd);
                                tScan.SetAttributeValue(reader.Name, reader.Value);
                            }
                        }
                    }
                }
            }
            return scans;
        }

        /// <summary>
        /// Read mzXML peaks property
        /// </summary>
        /// <param name="str"></param>
        /// <param name="peakCount"></param>
        /// <returns></returns>
        public static List<Centroid> ReadPeaks(string str,int peakCount) {
            Debug.WriteLine(str);
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
