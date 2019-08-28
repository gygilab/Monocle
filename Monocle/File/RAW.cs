using System;
using System.Collections.Generic;
using ThermoFisher.CommonCore.Data.Business;
using ThermoFisher.CommonCore.Data.FilterEnums;
using ThermoFisher.CommonCore.Data.Interfaces;
using ThermoFisher.CommonCore.RawFileReader;

namespace Monocle.File
{
    public class RAW : InputFile
    {
        
        public static List<Data.Scan> Consume(string rawFilePath, List<Data.Scan> scans)
        {
            if (rawFilePath == "" || !System.IO.File.Exists(rawFilePath))
            {
                Console.WriteLine(" Error: No file exists in the input.");
                return null;
            }

            ParentFile = rawFilePath;

            if (Ms1ScansCentroids == null || Ms1ScansCentroids.Length < 1)
            {
                Ms1ScansCentroids = new Data.Scan[Num_Ms1_Scans_To_Average];
            }

            try
            {
                IRawDataPlus rawFile = RawFileReaderAdapter.FileFactory(rawFilePath);
                if (!rawFile.IsOpen)
                {
                    Console.WriteLine(" RawFile Error: File could not be opened: " + rawFilePath);
                    Console.WriteLine(rawFile.FileError.WarningMessage);
                    Console.WriteLine(rawFile.FileError.ErrorMessage);
                    Console.WriteLine(rawFile.FileError.ErrorCode);
                    return null;
                }
                else if (rawFile.IsError)
                {
                    Console.WriteLine(" RawFile Error: reader error: " + rawFilePath);
                    return null;
                }

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
                    Data.Scan tempScan = new Data.Scan()
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
                        Polarity = (scanFilter.Polarity == PolarityType.Positive) ? "+" : "-",
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
                                tempScan.IonInjectionTime = double.Parse(trailer.Values[i]);
                                break;
                            case "Elapsed Scan Time (sec):":
                                tempScan.ElapsedScanTime = double.Parse(trailer.Values[i]);
                                break;
                            case "Monoisotopic M/Z:":
                                tempScan.MonoisotopicMz = double.Parse(trailer.Values[i]);
                                break;
                            case "Charge State:":
                                tempScan.PrecursorCharge = int.Parse(trailer.Values[i]);
                                break;
                            case "Master Scan Number:":
                                tempScan.PrecursorMasterScanNumber = int.Parse(trailer.Values[i]);
                                break;
                            case "FAIMS CV:":
                                tempScan.FaimsCV = (int)double.Parse(trailer.Values[i]);
                                break;
                        }
                    }

                    // Centroid or profile?:
                    if (scanStatistics.IsCentroidScan && (scanStatistics.SpectrumPacketType == SpectrumPacketType.FtCentroid))
                    {
                        // High res data
                        var centroidStream = rawFile.GetCentroidStream(iScanNumber, false);
                        PeakCount = centroidStream.Length;
                        tempScan.CentroidsFromArrays(centroidStream.Masses, centroidStream.Intensities);
                    }
                    else
                    {
                        // Low res data
                        var segmentedScan = rawFile.GetSegmentedScanFromScanNumber(iScanNumber, scanStatistics);
                        PeakCount = segmentedScan.Positions.Length;
                        tempScan.CentroidsFromArrays(segmentedScan.Positions, segmentedScan.Intensities);
                    }

                    scans.Add(tempScan);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" RAW File Error: " + ex.ToString());
            }
            Ms1ScansCentroids = new Data.Scan[12];
            Ms1ScanIndex = 0;
            return scans;
        }
    }
}
