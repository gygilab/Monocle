using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using Monocle.Data;

namespace Monocle.File
{
    public class MzMlWriter : IScanWriter
    {
        CryptoStream cryptoStream;

        SHA1 sha1;

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

        private readonly Dictionary <string, string> cvNames = new Dictionary<string, string>
        {
            { "MS:1000579", "MS1 spectrum" },
            { "MS:1000580", "MSn spectrum" },
            { "MS:1000768", "Thermo nativeID format" },
            { "MS:1000563", "Thermo RAW file" },
            { "MS:1000799", "custom unreleased software tool" },
            { "MS:1000569", "SHA-1" },
            { "MS:1000448", "LTQ FT" },
            { "MS:1000529", "instrument serial number" },
            { "MS:1000532", "Xcalibur" },
            { "MS:1000615", "ProteoWizard" },
            { "MS:1000073", "electrospray ionization" },
            { "MS:1000057", "electrospray inlet" },
            { "MS:1000079", "fourier transform ion cyclotron resonance mass spectrometer" },
            { "MS:1000624", "inductive detector" },
            { "MS:1000083", "radial ejection linear ion trap" },
            { "MS:1000253", "electron multiplier" },
            { "MS:1000544", "Conversion to mzML" },
            { "MS:1000511", "ms level" },
            { "MS:1000130", "positive scan" },
            { "MS:1000128", "profile spectrum" },
            { "MS:1000504", "base peak m/z" },
            { "MS:1000040", "m/z" },
            { "MS:1000505", "base peak intensity" },
            { "MS:1000131", "number of counts" },
            { "MS:1000285", "total ion current" },
            { "MS:1000528", "lowest observed m/z" },
            { "MS:1000527", "highest observed m/z" },
            { "MS:1000795", "no combination" },
            { "MS:1000016", "scan start time" },
            { "UO:0000031", "minute" },
            { "MS:1000512", "filter string" },
            { "MS:1000616", "preset scan configuration" },
            { "MS:1000501", "scan window lower limit" },
            { "MS:1000500", "scan window upper limit" },
            { "MS:1000523", "64-bit float" },
            { "MS:1000576", "no compression" },
            { "MS:1000514", "m/z array" },
            { "MS:1000521", "32-bit float" },
            { "MS:1000515", "intensity array" },
            { "MS:1000127", "centroid spectrum" },
            { "MS:1000633", "possible charge state" },
            { "MS:1000827", "isolation window target m/z" },
            { "MS:1000828", "isolation window lower offset" },
            { "MS:1000829", "isolation window upper offset" },
            { "MS:1000744", "selected ion m/z" },
            { "MS:1000133", "collision-induced dissociation" },
            { "MS:1000045", "collision energy" },
            { "UO:0000266", "electronvolt" },
            { "MS:1000235", "total ion current chromatogram" },
            { "MS:1000595", "time array" }
        };

        public void Open(string path) {
            scanIndex = new Dictionary<long, long>();

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\n";

            output = new StreamWriter(path);
            writer = XmlWriter.Create(output, settings);

            sha1 = SHA1.Create();
            cryptoStream = new CryptoStream(output.BaseStream, sha1, CryptoStreamMode.Write);
        }

        public void WriteHeader(ScanFileHeader header) {
            writer.WriteStartDocument();

            writer.WriteStartElement("indexedmzML", "http://psi.hupo.org/ms/mzml");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi", "schemaLocation", null, "http://psi.hupo.org/ms/mzml http://psidev.info/files/ms/mzML/xsd/mzML1.1.0.xsd");

            writer.WriteStartElement("mzML", "http://psi.hupo.org/ms/mzml");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi", "schemaLocation", null, "http://psi.hupo.org/ms/mzml http://psidev.info/files/ms/mzML/xsd/mzML1.1.0.xsd");
            writer.WriteAttributeString("version", "1.1.0");
            writer.WriteAttributeString("id", Path.GetFileNameWithoutExtension(header.FileName));

            writer.WriteStartElement("cvList");
            writer.WriteAttributeString("count", "2");
            
            writer.WriteStartElement("cv");
            writer.WriteAttributeString("id", "MS");
            writer.WriteAttributeString("fullName", "Proteomics Standards Initiative Mass Spectrometry Ontology");
            writer.WriteAttributeString("version", "1.18.2");
            writer.WriteAttributeString("URI", "http://psidev.cvs.sourceforge.net/*checkout*/psidev/psi/psi-ms/mzML/controlledVocabulary/psi-ms.obo");
            writer.WriteEndElement();

            writer.WriteStartElement("cv");
            writer.WriteAttributeString("id", "UO");
            writer.WriteAttributeString("fullName", "Unit Ontology");
            writer.WriteAttributeString("version", "04:03:2009");
            writer.WriteAttributeString("URI", "http://obo.cvs.sourceforge.net/*checkout*/obo/obo/ontology/phenotype/unit.obo");
            writer.WriteEndElement();
            writer.WriteEndElement();// cvlist 

            writer.WriteStartElement("fileDescription");
            writer.WriteStartElement("fileContent");

            WriteCVParam("MS:1000579", "");
            WriteCVParam("MS:1000580", "");

            writer.WriteEndElement(); // fileContent                
            writer.WriteEndElement(); // fileDescription        

            writer.WriteStartElement("softwareList");
            writer.WriteAttributeString("count", "1");

            writer.WriteStartElement("software");
            writer.WriteAttributeString("id", "Monocle");
            writer.WriteAttributeString("version", "1");

            WriteCVParam("MS:1000799", "custom unreleased software tool");

            writer.WriteEndElement(); // software                
            writer.WriteEndElement(); // softwareList                                                                                
        
            writer.WriteStartElement("instrumentConfigurationList");
            writer.WriteAttributeString("count", "1");

            writer.WriteStartElement("instrumentConfiguration");
            writer.WriteAttributeString("id", "IC1");

            writer.WriteStartElement("softwareRef");
            writer.WriteAttributeString("ref", "Xcalibur");
            writer.WriteEndElement(); // softwareRef       

            writer.WriteEndElement(); // instrumentConfiguration                
            writer.WriteEndElement(); // instrumentConfigurationList                

            writer.WriteStartElement("dataProcessingList");
            writer.WriteAttributeString("count", "1");

            writer.WriteStartElement("dataProcessing");
            writer.WriteAttributeString("id", "ThermoRawFileParserProcessing");

            writer.WriteStartElement("processingMethod");
            writer.WriteAttributeString("order", "0");
            writer.WriteAttributeString("softwareRef", "Monocle");

            WriteCVParam("MS:1000544", "");

            writer.WriteEndElement(); // processingMethod                
            writer.WriteEndElement(); // dataProcessing                
            writer.WriteEndElement(); // dataProcessingList                

            writer.WriteStartElement("run");
            writer.WriteAttributeString("id", Path.GetFileNameWithoutExtension(header.FileName));
            writer.WriteAttributeString("defaultInstrumentConfigurationRef", "IC1");
            writer.WriteAttributeString("defaultSourceFileRef", "sourcefile_1");

            writer.WriteStartElement("spectrumList");
            writer.WriteAttributeString("count", header.ScanCount.ToString());

            writer.WriteAttributeString("defaultDataProcessingRef", "ThermoRawFileParserProcessing");
        }

        public void WriteScan(Scan scan) {

            writer.WriteStartElement("spectrum");

            // Get position of spectrum tag for index.
            // Flush after writing since the previous tag isnt closed
            // until the spectrum tag is written.
            writer.Flush();
            long pos = output.BaseStream.Position - 9; // pos - length of "<spectrum"
            scanIndex.Add(scan.ScanNumber, pos);

            writer.WriteAttributeString("index", (scan.ScanNumber - 1).ToString());
            writer.WriteAttributeString("id", "scan=" + scan.ScanNumber.ToString());
            writer.WriteAttributeString("defaultArrayLength", scan.PeakCount.ToString());

            WriteCVParam("MS:1000511", scan.MsOrder.ToString());
            if (scan.MsOrder > 1) {
                WriteCVParam("MS:1000580", "");
            } else {
                WriteCVParam("MS:1000579", "");
            }

            switch(scan.Polarity) {
                case Polarity.Positive:
                    WriteCVParam("MS:1000130", "");
                    break;
                case Polarity.Negative:
                    WriteCVParam("MS:1000129", "");
                    break;
                default:
                    break;
            }

            WriteCVParam("MS:1000127", ""); // centroid spectrum
            WriteCVParam("MS:1000504", scan.BasePeakMz.ToString("G17", CultureInfo.InvariantCulture), "MS:1000040");
            WriteCVParam("MS:1000505", scan.BasePeakIntensity.ToString(), "MS:1000131");
            WriteCVParam("MS:1000285", scan.TotalIonCurrent.ToString());
            WriteCVParam("MS:1000528", scan.LowestMz.ToString("G17", CultureInfo.InvariantCulture), "MS:1000040");
            WriteCVParam("MS:1000527", scan.HighestMz.ToString("G17", CultureInfo.InvariantCulture), "MS:1000040");

            // No support for combined scans yet.
            writer.WriteStartElement("scanList");
            writer.WriteAttributeString("count", "1");

            WriteCVParam("MS:1000795", ""); // not a combined scan

            writer.WriteStartElement("scan");
            writer.WriteAttributeString("instrumentConfigurationRef", "IC1");

            // time in minutes
            WriteCVParam("MS:1000016", scan.RetentionTime.ToString(), "UO:0000031");
            WriteCVParam("MS:1000512", scan.FilterLine);

            writer.WriteStartElement("scanWindowList");
            writer.WriteAttributeString("count", "1");

            writer.WriteStartElement("scanWindow");
            WriteCVParam("MS:1000501", scan.StartMz.ToString("G17", CultureInfo.InvariantCulture), "MS:1000040");
            WriteCVParam("MS:1000500", scan.EndMz.ToString("G17", CultureInfo.InvariantCulture), "MS:1000040");
            writer.WriteEndElement(); // scanWindow

            writer.WriteEndElement(); // scanWindowList
            writer.WriteEndElement(); // scan
            writer.WriteEndElement(); // scanList

            if (scan.Precursors.Count > 0) {
                writer.WriteStartElement("precursorList");
                writer.WriteAttributeString("count", scan.Precursors.Count.ToString());

                foreach (var precursor in scan.Precursors) {
                    writer.WriteStartElement("precursor");
                    writer.WriteAttributeString("spectrumRef", "scan=" + scan.PrecursorMasterScanNumber.ToString());
                    
                    writer.WriteStartElement("isolationWindow");
                    WriteCVParam("MS:1000827", precursor.IsolationMz.ToString("G17", CultureInfo.InvariantCulture), "MS:1000040");
                    WriteCVParam("MS:1000828", precursor.IsolationWidth.ToString(), "MS:1000040");
                    WriteCVParam("MS:1000829", precursor.IsolationWidth.ToString(), "MS:1000040");
                    writer.WriteEndElement(); // isolationWindow

                    writer.WriteStartElement("selectedIonList");
                    writer.WriteAttributeString("count", "1");
                    writer.WriteStartElement("selectedIon");

                    WriteCVParam("MS:1000633", precursor.Charge.ToString());
                    WriteCVParam("MS:1000827", precursor.Mz.ToString("G17", CultureInfo.InvariantCulture), "MS:1000040");

                    writer.WriteEndElement(); // selectedIon
                    writer.WriteEndElement(); // selectedIonList

                    writer.WriteStartElement("activation");
                    WriteCVParam("MS:1000133", ""); // CID
                    WriteCVParam("MS:1000045", scan.CollisionEnergy.ToString(), "UO:0000266");
                    writer.WriteEndElement(); // activation

                    writer.WriteEndElement(); // precursor
                }
                writer.WriteEndElement(); // precursorList
            }

            writer.WriteStartElement("binaryDataArrayList");
            writer.WriteAttributeString("count", "2");

            string mzData = EncodePeaks(scan, false);
            writer.WriteStartElement("binaryDataArray");
            writer.WriteAttributeString("encodedLength", mzData.Length.ToString());
            WriteCVParam("MS:1000521", ""); // 32-bit float
            WriteCVParam("MS:1000576", ""); // no compression
            WriteCVParam("MS:1000514", "", "MS:1000040"); // m/z array
            writer.WriteStartElement("binary");
            writer.WriteString(mzData);
            writer.WriteEndElement(); // binary
            writer.WriteEndElement(); // binaryDataArray

            string intensityData = EncodePeaks(scan, true);
            writer.WriteStartElement("binaryDataArray");
            writer.WriteAttributeString("encodedLength", intensityData.Length.ToString());
            WriteCVParam("MS:1000521", ""); // 32-bit float
            WriteCVParam("MS:1000576", ""); // no compression
            WriteCVParam("MS:1000515", "", "MS:1000131"); // intensity array
            writer.WriteStartElement("binary");
            writer.WriteString(intensityData);
            writer.WriteEndElement(); // binary
            writer.WriteEndElement(); // binaryDataArray

            writer.WriteEndElement(); // binaryDataArrayList
            writer.WriteEndElement(); // spectrum
        }

        public void Close() {
            WriteIndex();
            writer.Close();
            output.Close();
            cryptoStream.Close();
        }

        private void WriteIndex() {
            writer.WriteEndElement(); // spectrumList
            writer.WriteEndElement(); // run
            writer.WriteEndElement(); // mzML

            writer.WriteStartElement("indexList");

            writer.Flush();
            long indexOffset = output.BaseStream.Position - 10; // Length of <indexList

            writer.WriteAttributeString("count", "1");

            writer.WriteStartElement("index");
            writer.WriteAttributeString("name", "spectrum");

            foreach(var entry in scanIndex) {
                writer.WriteStartElement("offset");
                writer.WriteAttributeString("idRef", "scan=" + entry.Key.ToString());
                writer.WriteString(entry.Value.ToString());
                writer.WriteEndElement(); // offset
            }

            writer.WriteEndElement(); // index

            writer.WriteEndElement(); // indexList


            writer.WriteStartElement("indexListOffset");
            writer.WriteString(indexOffset.ToString());
            writer.WriteEndElement(); // indexListOffset

            writer.WriteStartElement("fileChecksum");
            writer.WriteString("");

            writer.Flush();

            cryptoStream.FlushFinalBlock();
            var hash = sha1.Hash;
            sha1.Initialize();

            writer.WriteValue(BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant());
            writer.WriteEndElement(); // fileChecksum

            writer.WriteEndElement(); // indexedmzML

            writer.WriteEndDocument();
        }

        private void WriteCVParam(string accession, string value, string unitAccession="") {
            string cvRef = accession.Substring(0, 2);
            string name = cvNames[accession];

            writer.WriteStartElement("cvParam");
            writer.WriteAttributeString("cvRef", cvRef);
            writer.WriteAttributeString("accession", accession);
            writer.WriteAttributeString("name", name);
            writer.WriteAttributeString("value", value);

            if (unitAccession.Length > 0) {
                string unitCvRef = unitAccession.Substring(0, 2);
                string unitName = cvNames[unitAccession];

                writer.WriteAttributeString("unitCvRef", unitCvRef);
                writer.WriteAttributeString("unitAccession", unitAccession);
                writer.WriteAttributeString("unitName", unitName);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Encodes peak data in base64, 32bit, little-endian
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>
        private string EncodePeaks(Scan scan, bool intensity=false) {
            if (scan.PeakCount == 0) {
                return "AAAAAAAAAAA=";
            }
            
            // Allocate space for m/z and int pairs, four bytes each.
            byte[] bytes = new byte[scan.PeakCount * 4];

            for (int i = 0; i < scan.PeakCount; ++i) {
                var peak = scan.Centroids[i];
                double data;
                if (intensity) {
                    data = peak.Intensity;
                }
                else {
                    data = peak.Mz;
                }
                var mzBytes = BitConverter.GetBytes((float)data);
                mzBytes.CopyTo(bytes, i * 4);
            }

            return Convert.ToBase64String(bytes);
        }
    }
}
