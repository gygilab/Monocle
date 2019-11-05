
using Monocle.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Monocle.File {
    /// <summary>
    /// Writes the scan data in mzXML format.
    /// </summary>
    public class MzXmlWriter : IScanWriter
    {
        /// <summary>
        /// The XML serializer.
        /// </summary>
        private XmlWriter writer;

        /// <summary>
        /// The text output stream.
        /// The class holds on to this to remember the positions of the
        /// scans for creating the index at the end.
        /// </summary>
        private StreamWriter output;

        /// <summary>
        /// Stores the position of each scan for creating the index.
        /// Keys are scan numbers and values are the text position in the file.
        /// </summary>
        private Dictionary<long, long> scanIndex;

        /// <summary>
        /// Opens the file and initializes the XML stream.
        /// </summary>
        /// <param name="path"></param>
        public void Open(string path)
        {
            scanIndex = new Dictionary<long, long>();
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\n";

            output = new StreamWriter(path);
            writer = XmlWriter.Create(output, settings);
        }

        /// <summary>
        /// Writes the index and closing tags, 
        /// and closes the XML and output streams.
        /// </summary>
        public void Close()
        {
            WriteIndex();
            writer.Close();
            output.Close();
        }

        /// <summary>
        /// Writes a scan tag.
        /// </summary>
        /// <param name="scan"></param>
        public void WriteScan(Scan scan)
        {
            long pos = output.BaseStream.Position;
            scanIndex.Add(scan.ScanNumber, pos);

            writer.WriteStartElement("scan");
            writer.WriteAttributeString("num", scan.ScanNumber.ToString());
            writer.WriteAttributeString("msLevel", scan.MsOrder.ToString());
            writer.WriteAttributeString("scanEvent", scan.ScanEvent.ToString());
            writer.WriteAttributeString("masterIndex", scan.MasterIndex.ToString());
            writer.WriteAttributeString("peaksCount", scan.PeakCount.ToString());
            writer.WriteAttributeString("ionInjectionTime", scan.IonInjectionTime.ToString());
            writer.WriteAttributeString("elapsedScanTime", scan.ElapsedScanTime.ToString());
            writer.WriteAttributeString("polarity", scan.Polarity == Polarity.Positive ? "+" : "-");
            writer.WriteAttributeString("scanType", scan.ScanType.ToString());
            writer.WriteAttributeString("filterLine", scan.FilterLine);
            writer.WriteAttributeString("retentionTime", scan.RetentionTime.ToString());
            writer.WriteAttributeString("startMz", scan.StartMz.ToString());
            writer.WriteAttributeString("endMz", scan.EndMz.ToString());
            writer.WriteAttributeString("lowMz", scan.LowestMz.ToString());
            writer.WriteAttributeString("highMz", scan.HighestMz.ToString());
            writer.WriteAttributeString("basePeakMz", scan.BasePeakMz.ToString());
            writer.WriteAttributeString("basePeakIntensity", scan.BasePeakIntensity.ToString());

            writer.WriteStartElement("peaks");
            writer.WriteAttributeString("precision", "32");
            writer.WriteAttributeString("byteOrder", "network");
            writer.WriteAttributeString("contentType", "m/z-int");
            writer.WriteAttributeString("compressionType", "none");
            writer.WriteAttributeString("compressedLen", "0");
            writer.WriteString(EncodePeaks(scan));
            writer.WriteEndElement(); // peaks

            writer.WriteEndElement(); // scan
        }

        /// <summary>
        /// Writes the opening tags and metadata information.
        /// </summary>
        /// <param name="header"></param>
        public void WriteHeader(ScanFileHeader header)
        {
            writer.WriteStartElement("mzXML");
            writer.WriteAttributeString("xmlns", "http://sashimi.sourceforge.net/schema_revision/mzXML_3.1");
            writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi:schemaLocation", "http://sashimi.sourceforge.net/schema_revision/mzXML_3.1 http://sashimi.sourceforge.net/schema_revision/mzXML_3.1/mzXML_idx_3.1.xsd");

            writer.WriteStartElement("msRun");
            writer.WriteAttributeString("scanCount", header.ScanCount.ToString());
            writer.WriteAttributeString("startTtime", header.StartTime.ToString());
            writer.WriteAttributeString("endTime", header.EndTime.ToString());

            writer.WriteStartElement("parentFile");
            writer.WriteAttributeString("fileName", header.FileName);
            writer.WriteAttributeString("fileType", FileNameToType(header.FileName));
            writer.WriteEndElement(); // parentFile
            
            writer.WriteStartElement("msInstrument");
            writer.WriteStartElement("msManufacturer");
            writer.WriteAttributeString("category", "msManufacturer");
            writer.WriteAttributeString("value", header.InstrumentManufacturer);
            writer.WriteEndElement(); // msManufacturer
            writer.WriteStartElement("msModel");
            writer.WriteAttributeString("category", "msModel");
            writer.WriteAttributeString("value", header.InstrumentModel);
            writer.WriteEndElement(); // msModel
            writer.WriteEndElement(); // msInstrument
        }

        /// <summary>
        /// Writes the scan index and closes the tags for the
        /// end of the file.
        /// </summary>
        private void WriteIndex() 
        {
            writer.WriteEndElement(); // msRun

            long indexOffset = output.BaseStream.Position;
            writer.WriteStartElement("index");
            foreach(var entry in scanIndex) {
                writer.WriteStartElement("offset");
                writer.WriteAttributeString("id", entry.Key.ToString());
                writer.WriteString(entry.Value.ToString());
                writer.WriteEndElement(); // offset
            }
            writer.WriteEndElement(); // index

            writer.WriteStartElement("indexOffset");
            writer.WriteString(indexOffset.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement(); // mzXML
        }

        /// <summary>
        /// Encodes peak data in base64, 32bit, little-endian
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>
        private string EncodePeaks(Scan scan) {
            if (scan.PeakCount == 0) {
                return "AAAAAAAAAAA=";
            }
            
            // Allocate space for m/z and int pairs, four bytes each.
            byte[] bytes = new byte[scan.PeakCount * 2 * 4];

            for (int i = 0; i < scan.PeakCount; ++i) {
                var peak = scan.Centroids[i];
                var mzBytes = BitConverter.GetBytes(peak.Mz);
                Array.Reverse(mzBytes);
                mzBytes.CopyTo(bytes, i * 8);

                var intBytes = BitConverter.GetBytes(peak.Intensity);
                Array.Reverse(intBytes);
                intBytes.CopyTo(bytes, (i * 8) + 4);
            }
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Prepraes the string for the parentFile fileType attribute.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string FileNameToType(string name)
        {
            switch(Path.GetExtension(name).ToUpper()) {
                case "RAW":
                return "RawFile";
                case "MZXML":
                return "mzXML";
                default:
                break;
            }
            return "unknown";
        }
    }
}
