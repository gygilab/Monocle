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
            foreach (Scan scan in scans)
            {
                if (scan.MsOrder != Options.MS_Level)
                {
                    continue;
                }

                int window = Options.Number_Of_Scans_To_Average / 2;
                var NearbyMs1Scans = new List<Scan>(window * 2);
                int scanCount = 0;
                int index = scan.PrecursorMasterScanNumber - 1;
                if (Options.AveragingVector == AveragingVector.Before || Options.AveragingVector == AveragingVector.Both) {
                    // Reel backward.
                    for ( ; index >= 0 && scanCount < window; --index) {
                        if (scans[index].MsOrder == 1) {
                            ++scanCount;
                        }
                    }
                }

                // Collect scans.
                scanCount = 0;
                for ( ; index < scans.Count && scanCount < window; ++index) {
                    if (scans[index].MsOrder == 1) {
                        if (scans[index].ScanNumber > scan.PrecursorMasterScanNumber) {
                            if(Options.AveragingVector == AveragingVector.Before) {
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

        /// <summary>
        /// Run a single Monocle scan.
        /// </summary>
        /// <param name="Ms1ScansCentroids"></param>
        /// <param name="ParentScan"></param>
        /// <param name="DependentScan"></param>
        public static void Run(List<Scan> Ms1ScansCentroids, Scan ParentScan, Scan DependentScan, MonocleOptions Options)
        {
            try
            {
                double precursorMz = DependentScan.PrecursorMz; // This should be precursorMz or raw mono?
                int precursorCharge = DependentScan.PrecursorCharge;

                // For number of isotopes to consider
                int numIsotopes = 0;
                int monoisotopicIndex = 0;
                int numTheo = 4;
                int left = -7;

                // For charge detection
                int best_charge = 0;
                double best_score = -1;
                int bestIndex = monoisotopicIndex;
                double newMonoisotopicMz = 0;

                //Create new class to maintain ref class options
                ChargeRange chargeRange = new ChargeRange(precursorCharge, precursorCharge);
                if (Options.Charge_Detection)
                {
                    chargeRange.Low = Options.Charge_Range.Low;
                    chargeRange.High = Options.Charge_Range.High;
                }

                for (int charge_iterator = chargeRange.Low; charge_iterator <= chargeRange.High; charge_iterator++)
                {
                    double mass = DependentScan.PrecursorMz * charge_iterator;
                    // Restrict number of isotopes to consider based on precursor mass.
                    if (mass > 2900)
                    {
                        numIsotopes = 14;
                        left = -7;
                        numTheo = 7;
                    }
                    else if (mass > 1200)
                    {
                        numIsotopes = 10;
                        left = -5;
                        numTheo = 5;
                    }
                    else
                    {
                        numIsotopes = 7;
                        left = -3;
                        numTheo = 4;
                    }

                    monoisotopicIndex = -1 * left;

                    // Search for ion in parent scan, use parent ion mz for future peaks
                    int index = PeakMatcher.Match(ParentScan, precursorMz, 50, PeakMatcher.PPM);
                    if (index >= 0)
                    {
                        precursorMz = ParentScan.Centroids[index].Mz;
                    }

                    // Generate expected relative intensities.
                    List<double> expected = PeptideEnvelopeCalculator.GetTheoreticalEnvelope(precursorMz, charge_iterator, numTheo);
                    PeptideEnvelopeCalculator.Scale(expected);

                    PeptideEnvelope envelope = PeptideEnvelopeExtractor.Extract(Ms1ScansCentroids, precursorMz, charge_iterator, left, numIsotopes);

                    // Get best match using pearson correlation.
                    for (int i = 0; i < (numIsotopes - (expected.Count - 1)); ++i)
                    {
                        List<double> observed = envelope.averageIntensity.GetRange(i, expected.Count);
                        // Scale by numpeaks
                        PeptideEnvelopeCalculator.Scale(observed);
                        double p = Pearson.P(observed, expected);

                        if (p > best_score * 1.05)
                        {
                            // add 5% to give bias toward left peaks.
                            best_score = p;
                            if (p > 0.1)
                            {
                                // A peak to the left is included, so add
                                // offset to get monoisotopic index.
                                bestIndex = i + 1;
                                best_charge = charge_iterator;
                                newMonoisotopicMz = CalculateMz(envelope.mzs[bestIndex], envelope.intensities[bestIndex]);
                            }
                        }
                    }
                } // end charge for loop

                if (best_charge > 0)
                {
                    DependentScan.PrecursorCharge = best_charge;
                }

                newMonoisotopicMz = (newMonoisotopicMz == 0) ? precursorMz : newMonoisotopicMz;
                DependentScan.PrecursorMz = newMonoisotopicMz;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Run error: " + ex);
            }
        } // Run

        /// <summary>
        /// Calculate intensity weighted m/z for each precursor
        /// </summary>
        /// <param name="mzs"></param>
        /// <param name="intensities"></param>
        /// <returns></returns>
        public static double CalculateMz(List<double> mzs, List<double> intensities)
        {
            if (mzs.Count == 0)
            {
                return 0;
            }

            double totalWeightedMz = 0;
            double totalMz = 0;
            double totalIntensity = 0;
            for (int i = 0; i < mzs.Count; ++i)
            {
                totalWeightedMz += mzs[i] * intensities[i];
                totalMz += mzs[i];
                totalIntensity += intensities[i];
            }
            if (totalIntensity > 0)
            {
                return totalWeightedMz / totalIntensity;
            }
            return totalMz / mzs.Count;
        }
    }
}
