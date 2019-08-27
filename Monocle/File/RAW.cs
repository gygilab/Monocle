using Monocle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
                Console.WriteLine(" Error: No scans in the input.");
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
                if (!rawFile.IsOpen || rawFile.IsError)
                {
                    Console.WriteLine(" Error: unable to access the RAW file using the RawFileReader class.");
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
                    };

                    RunHeader runHeader = rawFile.RunHeader;
                    LogEntry trailer = rawFile.GetTrailerExtraInformation(iScanNumber);

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

                    // Check if MS1 and add to processing pool
                    if (scanFilter.MSOrder == MSOrderType.Ms)
                    {
                        ParentScan = Ms1ScansCentroids[Ms1ScanIndex] = tempScan;
                        Ms1ScanIndex++;
                    }
                    else if (scanFilter.MSOrder == MSOrderType.Ms2 || scanFilter.MSOrder == MSOrderType.Ms3 || scanFilter.MSOrder == MSOrderType.Ms4)
                    {
                        if (PeakCount > 0)
                        {
                            tempScan.PrecursorMz = rawFile.GetScanEventForScanNumber(iScanNumber).GetReaction(0).PrecursorMass;

                            var trailerData = rawFile.GetTrailerExtraInformation(iScanNumber);
                            for (int i = 0; i < trailerData.Length; i++)
                            {
                                switch (trailerData.Labels[i])
                                {
                                    case "Monoisotopic M/Z:":
                                        tempScan.MonoisotopicMz = double.Parse(trailerData.Values[i]);
                                        break;
                                    case "Charge State:":
                                        tempScan.PrecursorCharge = int.Parse(trailerData.Values[i]);
                                        break;
                                    case "Master Scan Number:":
                                        tempScan.PrecursorMasterScanNumber = int.Parse(trailerData.Values[i]);
                                        break;
                                    case "Orbitrap Resolution:":
                                        break;
                                }
                            }
                        }

                        //Monocle.Run(Ms1ScansCentroids, scans.Where(b => b.ScanNumber == tempScan.PrecursorMasterScanNumber).First(), ref tempScan);
                    }

                    scans.Add(tempScan);
                }
            } catch (Exception ex)
            {
                Console.WriteLine(" RAW File Error: " + ex.ToString());
            }
            Ms1ScansCentroids = new Data.Scan[12];
            Ms1ScanIndex = 0;
            return scans;
        }
    }
}
