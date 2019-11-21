using Monocle.Data;
using Monocle.Math;
using Monocle.Peak;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Monocle
{
    public static class Monocle
    {
        /// <summary>
        /// Overload to handle all available scans allowing for Ms1 inclusion of before + after
        /// </summary>
        /// <param name="AllScans"></param>
        /// <param name="DependentScan"></param>
        /// <param name="Number_Of_Scans_To_Average"></param>
        public static void Run(ref List<Scan> scans, MonocleOptions Options)
        {
            try
            {
                int window = Options.Number_Of_Scans_To_Average / 2;
                foreach (Scan scan in scans)
                {
                    if (scan.MsOrder != Options.MS_Level)
                    {
                        continue;
                    }

                    if (scan.PrecursorMasterScanNumber <= 0)
                    {
                        Console.WriteLine(String.Format("Scan {0} does not have a precursor scan number assigned.", scan.ScanNumber));
                        continue;
                    }

                    var NearbyMs1Scans = new List<Scan>(window * 2);
                    int scanCount = 0;
                    int index = scan.PrecursorMasterScanNumber - 1;
                    if (Options.AveragingVector == AveragingVector.Before || Options.AveragingVector == AveragingVector.Both)
                    {
                        // Reel backward.
                        for (; index > 0 && scanCount < window - 1; --index)
                        {
                            if (scans[index].MsOrder == 1)
                            {
                                ++scanCount;
                            }
                        }
                    }
                    scanCount = 0;
                    // Collect scans.
                    for (; index < scans.Count && scanCount < window; ++index)
                    {
                        if (scans[index].MsOrder == 1)
                        {
                            if (scans[index].ScanNumber > scan.PrecursorMasterScanNumber)
                            {
                                if (Options.AveragingVector == AveragingVector.Before)
                                {
                                    break;
                                }
                                ++scanCount;
                            }
                            NearbyMs1Scans.Add(scans[index]);
                        }
                    }
                    Scan precursorScan = scans[scan.PrecursorMasterScanNumber - 1];
                    Run(NearbyMs1Scans, precursorScan, scan, Options);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Monocle Run Error: " + ex);
            }
        }

        /// <summary>
        /// Run a single Monocle scan.
        /// </summary>
        /// <param name="Ms1ScansCentroids"></param>
        /// <param name="ParentScan"></param>
        /// <param name="DependentScan"></param>
        public static void Run(List<Scan> Ms1ScansCentroids, Scan ParentScan, Scan DependentScan, MonocleOptions Options)
        {
            double precursorMz = DependentScan.PrecursorMz; // This should be precursorMz or raw mono?
            int precursorCharge = DependentScan.PrecursorCharge;

            // For charge detection
            int bestCharge = 0;
            double bestScore = -1;
            int bestIndex = 0;
            List<double> bestPeaks = new List<double>();
            List<double> bestPeakIntensities = new List<double>();

            //Create new class to maintain ref class options
            ChargeRange chargeRange = new ChargeRange(precursorCharge, precursorCharge);
            if (Options.Charge_Detection)
            {
                chargeRange.Low = Options.Charge_Range.Low;
                chargeRange.High = Options.Charge_Range.High;
            }
            
            for (int charge = chargeRange.Low; charge <= chargeRange.High; charge++)
            {
                // Restrict number of isotopes to consider based on precursor mass.
                double mass = DependentScan.PrecursorMz * charge;
                var isotopeRange = new IsotopeRange(mass);

                // Search for ion in parent scan, use parent ion mz for future peaks
                int index = PeakMatcher.Match(ParentScan, precursorMz, 50, PeakMatcher.PPM);
                if (index >= 0)
                {
                    precursorMz = ParentScan.Centroids[index].Mz;
                }

                // Generate expected relative intensities.
                List<double> expected = PeptideEnvelopeCalculator.GetTheoreticalEnvelope(precursorMz, charge, isotopeRange.CompareSize);
                Vector.Scale(expected);

                PeptideEnvelope envelope = PeptideEnvelopeExtractor.Extract(Ms1ScansCentroids, precursorMz, charge, isotopeRange.Left, isotopeRange.Isotopes);

                // Get best match using dot product.
                for (int i = 0; i < (isotopeRange.Isotopes - (expected.Count - 1)); ++i)
                {
                    List<double> observed = envelope.averageIntensity.GetRange(i, expected.Count);
                    Vector.Scale(observed);
                    PeptideEnvelopeExtractor.ScaleByPeakCount(observed, envelope, i);
                    double score = Vector.Dot(observed, expected);

                    // add 5% to give bias toward left peaks.
                    if (score > bestScore * 1.05)
                    {
                        bestScore = score;
                        if (score > 0.1)
                        {
                            // A peak to the left is included, so add
                            // offset to get monoisotopic index.
                            bestIndex = i + 1;
                            bestCharge = charge;
                            bestPeaks = envelope.mzs[bestIndex];
                            bestPeakIntensities = envelope.mzs[bestIndex];
                        }
                    }
                }
            } // end charge for loop

            if (bestCharge > 0)
            {
                DependentScan.PrecursorCharge = bestCharge;
            }

            // Calculate m/z
            if (bestPeaks.Count > 0)
            {
                DependentScan.PrecursorMz = Vector.WeightedAverage(bestPeaks, bestPeakIntensities);
            }
            else
            {
                DependentScan.PrecursorMz = precursorMz;
            }
        }
    }
}
