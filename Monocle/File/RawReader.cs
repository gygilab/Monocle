using Monocle.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ThermoBiz = ThermoFisher.CommonCore.Data.Business;
using ThermoFisher.CommonCore.Data.FilterEnums;
using ThermoFisher.CommonCore.Data.Interfaces;
using ThermoFisher.CommonCore.RawFileReader;

namespace Monocle.File
{
    public class RawReader : IScanReader
    {
        /// <summary>
        /// Keeps track of the last ms1 scan
        /// for filling the scan event.
        /// </summary>
        private int LastMS1;

        private IRawDataPlus rawFile;

        /// <summary>
        /// Keep track of the children of each scan,
        /// so we can fill out parent scan information.
        /// </summary>
        private Dictionary<int, int> ScanParents = new Dictionary<int, int>();

        /// <summary>
        /// Open new Raw file with warning messages.
        /// </summary>
        /// <param name="path"></param>
        public void Open(string path)
        {
            rawFile = RawFileReaderAdapter.FileFactory(path);
            if (!rawFile.IsOpen)
            {
                Console.WriteLine(" RawFile Error: File could not be opened: " + path);
                Console.WriteLine(rawFile.FileError.WarningMessage);
                Console.WriteLine(rawFile.FileError.ErrorMessage);
                Console.WriteLine(rawFile.FileError.ErrorCode);
                throw new IOException("Failed to open RAW file.");
            }
            if (rawFile.IsError)
            {
                Console.WriteLine(" RawFile Error: reader error: " + path);
                throw new IOException("Error while opening RAW file.");
            }

            rawFile.SelectInstrument(ThermoBiz.Device.MS, 1);
        }

        public ScanFileHeader GetHeader()
        {
            var header = new ScanFileHeader();
            header.StartTime = (float) rawFile.RunHeaderEx.StartTime;
            header.EndTime = (float) rawFile.RunHeaderEx.EndTime;
            header.ScanCount = rawFile.RunHeaderEx.SpectraCount;
            header.InstrumentModel = rawFile.GetInstrumentData().Model;
            header.InstrumentManufacturer = "ThermoFisher";
            return header;
        }

        /// <summary>
        /// Dispose of the raw file when reading multiple files.
        /// </summary>
        public void Close()
        {
            rawFile.Dispose();
        }

        /// <summary>
        /// Open the given file and import scans into the reader.
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            rawFile.SelectInstrument(ThermoBiz.Device.MS, 1);

            // Get the first and last scan from the RAW file
            int FirstScan = rawFile.RunHeaderEx.FirstSpectrum;
            int LastScan = rawFile.RunHeaderEx.LastSpectrum;
            for (int iScanNumber = FirstScan; iScanNumber <= LastScan; iScanNumber++)
            {
                ThermoBiz.Scan thermoScan = ThermoBiz.Scan.FromFile(rawFile, iScanNumber);
                IScanFilter scanFilter = rawFile.GetFilterForScanNumber(iScanNumber);
                IScanEvent scanEvent = rawFile.GetScanEventForScanNumber(iScanNumber);

                if ((int)scanFilter.MSOrder == 1) {
                    LastMS1 = iScanNumber;
                }
                
                Data.Scan scan = new Data.Scan()
                {
                    ScanNumber = iScanNumber,
                    ScanEvent = (iScanNumber - LastMS1) + 1,
                    BasePeakIntensity = thermoScan.ScanStatistics.BasePeakIntensity,
                    BasePeakMz = thermoScan.ScanStatistics.BasePeakMass,
                    TotalIonCurrent = thermoScan.ScanStatistics.TIC,
                    LowestMz = thermoScan.ScanStatistics.LowMass,
                    HighestMz = thermoScan.ScanStatistics.HighMass,
                    StartMz = thermoScan.ScanStatistics.LowMass,
                    EndMz = thermoScan.ScanStatistics.HighMass,
                    ScanType = ReadScanType(scanFilter.ToString()),
                    MsOrder = (int)scanFilter.MSOrder,
                    Polarity = (scanFilter.Polarity == PolarityType.Positive) ? Data.Polarity.Positive : Data.Polarity.Negative,
                    FilterLine = scanFilter.ToString(),
                    RetentionTime = rawFile.RetentionTimeFromScanNumber(iScanNumber)
                };

                if(scan.MsOrder > 1)
                {
                    // Get the current scan's activation method while ignoring upstream activation
                    scan.PrecursorActivationMethod = ConvertActivationType(scanFilter.GetActivation(scan.MsOrder - 2));

                    // handle dependent scans and not SPS (processed below)
                    scan.Precursors.Clear();
                    for (int i = 0; i < scanEvent.MassCount; ++i){
                        var reaction = scanEvent.GetReaction(i);
                        scan.CollisionEnergy = reaction.CollisionEnergy;

                        var precursor = new Data.Precursor
                        {
                            IsolationWidth = reaction.IsolationWidth,
                            IsolationMz = reaction.PrecursorMass,
                            Mz = reaction.PrecursorMass,
                            OriginalMz = reaction.PrecursorMass
                        };
                        scan.Precursors.Add(precursor);
                    }
                }

                ThermoBiz.RunHeader runHeader = rawFile.RunHeader;
                ThermoBiz.LogEntry trailer = rawFile.GetTrailerExtraInformation(iScanNumber);
                for (int i = 0; i < trailer.Length; i++)
                {
                    var value = trailer.Values[i];
                    if (value == null)
                    {
                        continue;
                    }
                    switch (trailer.Labels[i])
                    {
                        case "Access ID":
                            int access_id = Convert.ToInt32(value);
                            if(access_id > 0) {
                                scan.PrecursorMasterScanNumber = access_id;
                            }
                            break;
                        case "Scan Description:":
                            scan.Description = value.Trim();
                            break;
                        case "Ion Injection Time (ms):":
                            scan.IonInjectionTime = double.Parse(value);
                            break;
                        case "Elapsed Scan Time (sec):":
                            scan.ElapsedScanTime = double.Parse(value);
                            break;
                        case "Charge State:":
                            int charge = int.Parse(value);
                            foreach (var precursor in scan.Precursors) {
                                precursor.Charge = charge;
                                precursor.OriginalCharge = precursor.Charge;
                            }
                            break;
                        case "Master Scan Number:":
                            scan.PrecursorMasterScanNumber = int.Parse(value);
                            break;
                        case "Master Index:":
                            scan.MasterIndex = int.Parse(value);
                            break;
                        case "FAIMS CV:":
                            scan.FaimsCV = (int)double.Parse(value);
                            break;
                        case "FAIMS Voltage On:":
                            scan.FaimsState = (value == "No") ? Data.TriState.Off : Data.TriState.On;
                            break;
                        case "SPS Masses:":
                            string[] spsIonStringArray = value.TrimEnd(',').Split(',');
                            if(!string.IsNullOrWhiteSpace(spsIonStringArray[0]) && spsIonStringArray.Length > 0)
                            {
                                scan.Precursors.Clear();
                                for (int spsIndex = 0; spsIndex < spsIonStringArray.Length; spsIndex++)
                                {
                                    if (double.TryParse(spsIonStringArray[spsIndex], out double spsIon))
                                    {
                                        scan.Precursors.Add(new Data.Precursor(spsIon, 0, 1));
                                    }
                                }
                            }
                            break;
                    }
                }
                
                if (scan.PrecursorMasterScanNumber <= 0 && scan.MsOrder > 1) {
                    // Try again to set the precursor scan.
                    SetPrecursorScanNumber(scan);
                }

                if (scan.MsOrder > 1 && scan.PrecursorMasterScanNumber > rawFile.RunHeader.FirstSpectrum && scan.PrecursorMasterScanNumber < rawFile.RunHeader.LastSpectrum)
                {
                    // Fill precursor information
                    var parentScan = ThermoBiz.Scan.FromFile(rawFile, scan.PrecursorMasterScanNumber);
                    if (parentScan != null) {
                        foreach(var precursor in scan.Precursors) {
                            precursor.Intensity = GetMaxIntensity(parentScan, precursor.IsolationMz, precursor.IsolationWidth);
                        }
                    }
                }

                if (!thermoScan.ScanStatistics.IsCentroidScan) {
                    // Convert profile to centroid
                    thermoScan = ThermoBiz.Scan.ToCentroid(thermoScan);
                }

                if (thermoScan.HasCentroidStream) {
                    // High res data
                    CentroidsFromArrays(scan, thermoScan.CentroidScan.Masses, thermoScan.CentroidScan.Intensities, thermoScan.CentroidScan.Baselines, thermoScan.CentroidScan.Noises);
                }
                else {
                    // Low res data
                    CentroidsFromArrays(scan, thermoScan.PreferredMasses, thermoScan.PreferredIntensities);
                }

                if (scan.PeakCount > 0) {
                    scan.LowestMz = scan.Centroids[0].Mz;
                    scan.HighestMz = scan.Centroids[scan.PeakCount - 1].Mz;
                }

                yield return scan;
            }
        }

        /// <summary>
        /// Generate centroids from two arrays of m/z and intensity
        /// </summary>
        /// <param name="scan"></param>
        /// <param name="mzArray"></param>
        /// <param name="intensityArray"></param>
        public void CentroidsFromArrays(Data.Scan scan, double[] mzArray, double[] intensityArray, double[] baseline=null, double[] noise=null)
        {
            if(mzArray.Length != intensityArray.Length)
            {
                throw new Exception(" Error: MZ and Intensity Arrays of unequal length.");
            }
            scan.PeakCount = mzArray.Length;
            for (int i = 0; i < mzArray.Length; i++)
            {
                Centroid tempCentroid = new Centroid()
                {
                    Mz = mzArray[i],
                    Intensity = intensityArray[i],
                };
                if(baseline != null)
                {
                    tempCentroid.Baseline = baseline[i];
                }
                if(noise != null)
                {
                    tempCentroid.Noise = noise[i];
                }
                scan.Centroids.Add(tempCentroid);
            }
        }

        /// <summary>
        /// Converts the ActivationType Enum to a string
        /// </summary>
        /// <returns>The activation type.</returns>
        /// <param name="type">Type.</param>
        private static string ConvertActivationType(ActivationType type)
        {
            string output = "";
            switch (type)
            {
                case ActivationType.CollisionInducedDissociation:
                    output = "CID";
                    break;
                case ActivationType.ElectronCaptureDissociation:
                    output = "ECD";
                    break;
                case ActivationType.ElectronTransferDissociation:
                    output = "ETD";
                    break;
                case ActivationType.HigherEnergyCollisionalDissociation:
                    output = "HCD";
                    break;
                case ActivationType.PQD:
                    output = "PQD";
                    break;
                case ActivationType.UltraVioletPhotoDissociation:
                    output = "UVPD";
                    break;
            }
            return output;
        }

        private static Regex scanTypeRegex = new Regex (@" (\S+) ms\d? ", RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets the scan type by parsing it out of the filterline.
        /// </summary>
        /// <returns>The scan type.</returns>
        /// <param name="filterLine">Filter line.</param>
        private static string ReadScanType (string filterLine)
        {
            var m = scanTypeRegex.Match (filterLine);
            if (m.Success) {
                return m.Groups [1].ToString ();
            }
            return "";
        }

        /// <summary>
        /// Read the precursor scan number for a given scan.
        /// This is a fallback method since it's preferred to get the value
        /// from the "Master Scan Number" field in the scan header.
        /// </summary>
        /// <param name="scan">The scan that needs the assignment of the parent scan number</param>
        private void SetPrecursorScanNumber(Data.Scan scan)
        {
            if (ScanParents.Count == 0) {
                // Populate the index - this can be slow.
                ReadScanParents();
            }
            if (ScanParents.ContainsKey (scan.ScanNumber)) {
                scan.PrecursorMasterScanNumber = ScanParents[scan.ScanNumber];
            }
        }

        /// <summary>
        /// Populate information about parent scan numbers
        /// </summary>
        private void ReadScanParents()
        {
            int firstScan = rawFile.RunHeaderEx.FirstSpectrum;
            int lastScan = rawFile.RunHeaderEx.LastSpectrum;
            for (int scanNumber = firstScan; scanNumber <= lastScan; scanNumber++) {
                var dependents = rawFile.GetScanDependents(scanNumber, 1);
                if (dependents != null) {
                    foreach (var depedent in dependents.ScanDependentDetailArray) {
                        ScanParents[depedent.ScanIndex] = scanNumber;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the max intensity found by looking through the peaks in the parent scan.
        /// </summary>
        /// 
        /// <returns>The max intensity.</returns>
        /// <param name="number">the scan number.</param>
        /// <param name="targetMz">Target mz.</param>
        private double GetMaxIntensity (ThermoBiz.Scan scan, double targetMz, double isolationWidth)
        {
            bool hasCentroidStream = scan.ScanStatistics.IsCentroidScan && (scan.ScanStatistics.SpectrumPacketType == ThermoBiz.SpectrumPacketType.FtCentroid);
            double tolerance = 0.5;
            if (isolationWidth > 0.01)
            {   
                tolerance = isolationWidth / 2;
            }
            else if (hasCentroidStream)
            {   
                tolerance = 0.05;
            }
            double precursorIntensity = 0;
            double[] mzs = null;
            double[] intensities = null;
            if (hasCentroidStream) {
                var centroidStream = scan.CentroidScan;
                mzs = centroidStream.Masses;
                intensities = centroidStream.Intensities;
            } else {
                var segmentedScan = scan.SegmentedScan;
                mzs = segmentedScan.Positions;
                intensities = segmentedScan.Intensities;
            }

            // Find the nearest peak to the low end.
            int low = 0;
            int mid = 0;
            int hi = 0;
            double lowerMz = targetMz - tolerance;
            while (hi > low) {
                mid = (int) System.Math.Ceiling((hi - low) / 2.0) + low;
                if (mzs[mid] > lowerMz) {
                    hi = mid - 1;
                }
                else {
                    low = mid;
                }
            }

            // Iterate until it reaches the upper bound.
            for (int i = mid; i < mzs.Length; i++) {
                double mz = mzs[i];
                double intensity = intensities[i];
                if (System.Math.Abs(targetMz - mz) < tolerance) {
                    if (intensity > precursorIntensity) {
                        precursorIntensity = intensity;
                    }
                }
                // stop if we've gone too far
                if (mz - targetMz > tolerance) {
                    break;
                }
            }

            return precursorIntensity;
        }

    }
}
