using Monocle.Data;
using System;
using System.Collections.Generic;
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
                    ScanEvent = iScanNumber,
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
                            scan.MonoisotopicMz = double.Parse(trailer.Values[i]);
                            break;
                        case "Charge State:":
                            scan.PrecursorCharge = int.Parse(trailer.Values[i]);
                            break;
                        case "Master Scan Number:":
                            scan.PrecursorMasterScanNumber = int.Parse(trailer.Values[i]);
                            break;
                        case "FAIMS CV:":
                            scan.FaimsCV = (int)double.Parse(trailer.Values[i]);
                            break;
                        case "SPS Masses:":
                            scan.SpsIonsString = trailer.Values[i];
                            break;
                    }
                }

                IScanEvent scanEvent = rawFile.GetScanEventForScanNumber(iScanNumber);
                //write current scan filter:
                //Console.WriteLine("Scan " + iScanNumber + ": " + scanEvent.ToString());
                // handle dependent scans and not SPS (processed above)
                if (scan.MsOrder > 1 && scan.SpsIonsString == "")
                {
                    IReaction reaction = scanEvent.GetReaction(0);
                    scan.PrecursorMz = reaction.PrecursorMass;
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


    }
}
