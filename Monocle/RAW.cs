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

namespace Monocle
{
    public class RAW : InputFile
    {
        
        public static List<Scan> Consume(string rawFilePath, List<Scan> scans)
        {
            if (rawFilePath == "" || !File.Exists(rawFilePath))
            {
                Console.WriteLine(" Error: No scans in the input.");
                return null;
            }

            ParentFile = rawFilePath;

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
                    Scan tempScan = new Scan()
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

                    if (PeakCount > 0)
                    {
                        tempScan.PrecursorMz = rawFile.GetScanEventForScanNumber(iScanNumber).GetReaction(0).PrecursorMass;

                        var trailerData = rawFile.GetTrailerExtraInformation(iScanNumber);
                        for (int i = 0; i < trailerData.Length; i++)
                        {
                            if (trailerData.Labels[i] == "Monoisotopic M/Z:")
                                tempScan.MonoisotopicMz = double.Parse(trailerData.Values[i]);
                            else if (trailerData.Labels[i] == "Charge State:")
                                tempScan.PrecursorCharge = (int)double.Parse(trailerData.Values[i]);
                        }
                    }

                    // Check if MS1 and add to processing pool
                    if (scanFilter.MSOrder == MSOrderType.Ms)
                    {
                        ParentScan = Ms1ScansCentroids[Ms1ScanIndex] = tempScan;
                        Ms1ScanIndex++;
                    }
                    else if (scanFilter.MSOrder == MSOrderType.Ms2)
                    {
                        Monocle.Run(ref Ms1ScansCentroids, scans.Where(b => b.ScanNumber == tempScan.PrecursorMasterScanNumber).First(), ref tempScan);
                    }

                    scans.Add(tempScan);
                }
            } catch (Exception ex)
            {
                Console.WriteLine(" RAW File Error: " + ex.ToString());
            }
            Ms1ScansCentroids = new Scan[12];
            Ms1ScanIndex = 0;
            return scans;
        }

    }
}
