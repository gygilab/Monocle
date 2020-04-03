using Monocle.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ThermoFisher.CommonCore.Data.Business;
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

            rawFile.SelectInstrument(Device.MS, 1);
            ReadScanParents();
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
            rawFile.SelectInstrument(Device.MS, 1);

            // Get the first and last scan from the RAW file
            int FirstScan = rawFile.RunHeaderEx.FirstSpectrum;
            int LastScan = rawFile.RunHeaderEx.LastSpectrum;
            for (int iScanNumber = FirstScan; iScanNumber <= LastScan; iScanNumber++)
            {
                ScanStatistics scanStatistics = rawFile.GetScanStatsForScanNumber(iScanNumber);
                // Get the scan filter for this scan number
                IScanFilter scanFilter = rawFile.GetFilterForScanNumber(iScanNumber);
                IScanEvents scanEvents = rawFile.ScanEvents;

                if ((int)scanFilter.MSOrder == 1) {
                    LastMS1 = iScanNumber;
                }

                Data.Scan scan = new Data.Scan()
                {
                    ScanNumber = iScanNumber,
                    ScanEvent = (iScanNumber - LastMS1) + 1,
                    BasePeakIntensity = scanStatistics.BasePeakIntensity,
                    BasePeakMz = scanStatistics.BasePeakMass,
                    TotalIonCurrent = scanStatistics.TIC,
                    LowestMz = scanStatistics.LowMass,
                    HighestMz = scanStatistics.HighMass,
                    StartMz = scanStatistics.LowMass,
                    EndMz = scanStatistics.HighMass,
                    ScanType = ReadScanType(scanFilter.ToString()),
                    MsOrder = (int)scanFilter.MSOrder,
                    Polarity = (scanFilter.Polarity == PolarityType.Positive) ? Data.Polarity.Positive : Data.Polarity.Negative,
                    FilterLine = scanFilter.ToString(),
                    RetentionTime = rawFile.RetentionTimeFromScanNumber(iScanNumber)
                };
                // Get the current scan's activation method while ignoring upstream activation
                if(scan.MsOrder > 1)
                {
                    scan.PrecursorActivationMethod = ConvertActivationType(scanFilter.GetActivation(scan.MsOrder - 2));
                    if (ScanParents.ContainsKey (scan.ScanNumber)) {
                        scan.PrecursorMasterScanNumber = ScanParents[scan.ScanNumber];
                    }
                }

                IScanEvent scanEvent = rawFile.GetScanEventForScanNumber(iScanNumber);

                // handle dependent scans and not SPS (processed below)
                if (scan.MsOrder > 1)
                {
                    scan.Precursors.Clear();
                    for (int i = 0; i < scanEvent.MassCount; ++i){
                        var reaction = scanEvent.GetReaction(i);
                        scan.CollisionEnergy = reaction.CollisionEnergy;

                        var precursor = new Data.Precursor();
                        precursor.IsolationWidth = reaction.IsolationWidth;
                        precursor.IsolationMz = reaction.PrecursorMass;
                        precursor.Mz = reaction.PrecursorMass;
                        precursor.OriginalMz = precursor.Mz;
                        scan.Precursors.Add(precursor);
                    }
                }

                RunHeader runHeader = rawFile.RunHeader;
                LogEntry trailer = rawFile.GetTrailerExtraInformation(iScanNumber);
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
                            // Legacy implementation of master scan number
                            if(scan.PrecursorMasterScanNumber == 0) {
                                scan.PrecursorMasterScanNumber = int.Parse(value);
                            }
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

                // Fill precursor information
                // after getting the parent scan and header information.
                if (scan.MsOrder > 1)
                {
                    foreach(var precursor in scan.Precursors) {
                        precursor.Intensity = GetMaxIntensity(ScanParents[scan.ScanNumber], precursor.IsolationMz, precursor.IsolationWidth);
                    }
                }

                // Centroid or profile?:
                if (scanStatistics.IsCentroidScan && (scanStatistics.SpectrumPacketType == SpectrumPacketType.FtCentroid))
                {
                    // High res data
                    var centroidStream = rawFile.GetCentroidStream(iScanNumber, true);
                    CentroidsFromArrays(scan, centroidStream.Masses, centroidStream.Intensities, centroidStream.Baselines, centroidStream.Noises);
                }
                else
                {
                    // Low res data
                    var segmentedScan = rawFile.GetSegmentedScanFromScanNumber(iScanNumber, scanStatistics);
                    CentroidsFromArrays(scan, segmentedScan.Positions, segmentedScan.Intensities);
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
        private double GetMaxIntensity (int number, double targetMz, double isolationWidth)
        {
            ScanStatistics scanStatistics = rawFile.GetScanStatsForScanNumber(number);

            bool hasCentroidStream = scanStatistics.IsCentroidScan && (scanStatistics.SpectrumPacketType == SpectrumPacketType.FtCentroid);
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
                var centroidStream = rawFile.GetCentroidStream(number, true);
                mzs = centroidStream.Masses;
                intensities = centroidStream.Intensities;
            } else {
                var segmentedScan = rawFile.GetSegmentedScanFromScanNumber(number, scanStatistics);
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
