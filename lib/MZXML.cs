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
        public static Scan[] Ms1ScansCentroids = new Scan[12];
        public static Scan ParentScan = new Scan();
        private static int _Ms1ScanIndex { get; set; } = 0;
        public static int Ms1ScanIndex
        {
            get
            {
                return _Ms1ScanIndex;
            }
            set
            {
                if (value >= Ms1ScansCentroids.Length)
                {
                    _Ms1ScanIndex = 0;
                }
                else
                {
                    _Ms1ScanIndex = value;
                }
            }
        }

        public static List<Scan> Process(string xmlFilePath, List<Scan> scans)
        {
            if (xmlFilePath == "" || !File.Exists(xmlFilePath))
            {
                Debug.WriteLine("No scans in the input.");
                return null;
            }
            XmlDocument doc = new XmlDocument();
            using (FileStream fs = new FileStream(xmlFilePath, FileMode.Open))
            {
                doc.Load(fs);
            }
            using (XmlNodeList scanElems = doc.GetElementsByTagName("scan"))
            {
                foreach (XmlNode node in scanElems)
                {
                    Scan tScan = new Scan();
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        tScan.CheckAndSetValue(attr.Name, attr.Value);
                    }

                    //Process child nodes
                    XmlNodeList children = node.ChildNodes;
                    foreach (XmlNode child in children)
                    {
                        tScan.CheckAndSetValue(child.Name, child.InnerText);
                        foreach (XmlAttribute attr in child.Attributes)
                        {
                            tScan.CheckAndSetValue(attr.Name, attr.Value);
                        }
                    }
                    // Check if MS1 and add to processing pool
                    if (tScan.MsOrder == 1)
                    {
                        ParentScan = Ms1ScansCentroids[Ms1ScanIndex] = tScan;
                        Ms1ScanIndex++;
                    }
                    else if (tScan.MsOrder == 2)
                    {
                        Monocle.Run(ref Ms1ScansCentroids, scans.Where(b => b.ScanNumber == tScan.PrecursorMasterScanNumber).First(), ref tScan);
                    }

                    scans.Add(tScan);
                }
            }
            Ms1ScansCentroids = new Scan[12];
            Ms1ScanIndex = 0;
            return scans;
        }

        public static void Write(string xmlFilePath, List<Scan> scans)
        {
            if (xmlFilePath == "" || !File.Exists(xmlFilePath))
            {
                Debug.WriteLine("No proteins in the input.");
            }
            XmlDocument doc = new XmlDocument();
            doc.BuildInitialMzxml("");
            foreach(Scan scan in scans)
            {
                doc.ScanToXml(scan);
            }
            doc.Save(Files.ExportPath + "test.mzXML");
            Ms1ScansCentroids = new Scan[12];
            Ms1ScanIndex = 0;
            Debug.WriteLine("Finished writing xml: " + Files.ExportPath + "\\test.mzXML");
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
