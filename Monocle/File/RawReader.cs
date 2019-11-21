using Monocle.Data;
using System;
using System.IO;
using ThermoFisher.CommonCore.Data.Business;
using ThermoFisher.CommonCore.Data.FilterEnums;
using ThermoFisher.CommonCore.Data.Interfaces;
using ThermoFisher.CommonCore.RawFileReader;

namespace Monocle.File
{
    public class RawReader : IScanReader
    {
        private IRawDataPlus rawFile;
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
            int PeakCount = 0;
            for (int iScanNumber = FirstScan; iScanNumber <= LastScan; iScanNumber++)
            {
                ScanStatistics scanStatistics = rawFile.GetScanStatsForScanNumber(iScanNumber);
                // Get the scan filter for this scan number
                IScanFilter scanFilter = rawFile.GetFilterForScanNumber(iScanNumber);
                IScanEvents scanEvents = rawFile.ScanEvents;

                Data.Scan scan = new Data.Scan()
                {
                    ScanNumber = iScanNumber,
                    ScanEvent = scanStatistics.ScanEventNumber,
                    BasePeakIntensity = scanStatistics.BasePeakIntensity,
                    BasePeakMz = scanStatistics.BasePeakMass,
                    TotalIonCurrent = scanStatistics.TIC,
                    LowestMz = scanStatistics.LowMass,
                    HighestMz = scanStatistics.HighMass,
                    StartMz = scanStatistics.LowMass,
                    EndMz = scanStatistics.HighMass,
                    ScanType = scanStatistics.ScanType,
                    MsOrder = (int)scanFilter.MSOrder,
                    Polarity = (scanFilter.Polarity == PolarityType.Positive) ? Data.Polarity.Positive : Data.Polarity.Negative,
                    FilterLine = scanFilter.ToString(),
                    RetentionTime = rawFile.RetentionTimeFromScanNumber(iScanNumber)
                };
                // Get the current scan's activation method while ignoring upstream activation
                if(scan.MsOrder > 1)
                {
                    scan.PrecursorActivationMethod = ConvertActivationType(scanFilter.GetActivation(scan.MsOrder - 2));
                }

                IScanEvent scanEvent = rawFile.GetScanEventForScanNumber(iScanNumber);

                // handle dependent scans and not SPS (processed below)
                if (scan.MsOrder > 1)
                {
                    scan.Precursors.Clear();
                    for (int i = 0; i < scanEvent.MassCount; ++i){
                        var reaction = scanEvent.GetReaction(i);
                        var precursor = new Data.Precursor();
                        precursor.IsolationWidth = reaction.IsolationWidth;
                        precursor.IsolationMz = reaction.PrecursorMass;
                        precursor.Mz = reaction.PrecursorMass;
                        scan.Precursors.Add(precursor);
                    }
                }

                RunHeader runHeader = rawFile.RunHeader;
                LogEntry trailer = rawFile.GetTrailerExtraInformation(iScanNumber);
                for (int i = 0; i < trailer.Length; i++)
                {
                    if (trailer.Values[i] == null)
                    {
                        continue;
                    }
                    switch (trailer.Labels[i])
                    {
                        case "Ion Injection Time (ms):":
                            scan.IonInjectionTime = double.Parse(trailer.Values[i]);
                            break;
                        case "Elapsed Scan Time (sec):":
                            scan.ElapsedScanTime = double.Parse(trailer.Values[i]);
                            break;
                        case "Monoisotopic M/Z:":
                            scan.PrecursorMz = double.Parse(trailer.Values[i]);
                            break;
                        case "Charge State:":
                            scan.PrecursorCharge = int.Parse(trailer.Values[i]);
                            break;
                        case "Master Scan Number:":
                            scan.PrecursorMasterScanNumber = int.Parse(trailer.Values[i]);
                            break;
                        case "Master Index:":
                            // Legacy implementation of master scan number
                            scan.PrecursorMasterScanNumber = int.Parse(trailer.Values[i]);
                            break;
                        case "FAIMS CV:":
                            scan.FaimsCV = (int)double.Parse(trailer.Values[i]);
                            break;
                        case "SPS Masses:":
                            string[] spsIonStringArray = trailer.Values[i].TrimEnd(',').Split(',');
                            if(!string.IsNullOrWhiteSpace(spsIonStringArray[0]) && spsIonStringArray.Length > 0)
                            {
                                scan.Precursors.Clear();
                                for (int spsIndex = 0; spsIndex < spsIonStringArray.Length; spsIndex++)
                                {
                                    if (double.TryParse(spsIonStringArray[spsIndex], out double spsIon))
                                    {
                                        scan.Precursors.Add(new Data.Precursor(spsIon));
                                    }
                                }
                            }
                            break;
                    }
                }

                // Centroid or profile?:
                if (scanStatistics.IsCentroidScan && (scanStatistics.SpectrumPacketType == SpectrumPacketType.FtCentroid))
                {
                    // High res data
                    var centroidStream = rawFile.GetCentroidStream(iScanNumber, false);
                    PeakCount = centroidStream.Length;
                    CentroidsFromArrays(scan, centroidStream.Masses, centroidStream.Intensities);
                }
                else
                {
                    // Low res data
                    var segmentedScan = rawFile.GetSegmentedScanFromScanNumber(iScanNumber, scanStatistics);
                    PeakCount = segmentedScan.Positions.Length;
                    CentroidsFromArrays(scan, segmentedScan.Positions, segmentedScan.Intensities);
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
        public void CentroidsFromArrays(Data.Scan scan, double[] mzArray, double[] intensityArray)
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

    }
}
