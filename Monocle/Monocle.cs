using Monocle.Data;
using Monocle.Math;
using Monocle.Peak;
using System.Collections.Generic;
using System.Linq;

namespace Monocle
{
    public static class Monocle
    {
        /// <summary>
        /// Flexible options stack to run monocle
        /// </summary>
        public class MonocleOptions
        {
            public int Number_Of_Scans_To_Average { get; set; } = 12;
            public AveragingVector AveragingVector { get; set; } = AveragingVector.Both;
            public bool Charge_Detection { get; set; } = false;
            public IntRange Charge_Range_LowRes { get; set; }
        }

        public enum AveragingVector
        {
            Before,
            After,
            Both
        }

        /// <summary>
        /// Overload to handle all available scans alowing for Ms1 inclusion of before + after
        /// </summary>
        /// <param name="AllScans"></param>
        /// <param name="DependentScan"></param>
        /// <param name="Number_Of_Scans_To_Average"></param>
        public static void Run(ref List<Scan> AllScans, MonocleOptions Options)
        {
            foreach (Scan scan in AllScans)
            {
                if(scan.MsOrder == 1)
                {
                    continue;
                }
                int masterScanNumber = scan.PrecursorMasterScanNumber;
                int currentScanNumber = scan.ScanNumber;
                Scan PrecursorScan = AllScans.Where(b => b.PrecursorMasterScanNumber == masterScanNumber).First();
                List<Scan> NearbyMs1Scans = new List<Scan>();
                int plusMinusN = (Options.Number_Of_Scans_To_Average / 2);
                if (Options.AveragingVector == AveragingVector.Both)
                {
                    NearbyMs1Scans = AllScans.Where(c => c.MsOrder == 1 && c.ScanNumber > currentScanNumber).OrderBy(b => b.ScanNumber - currentScanNumber).Take(plusMinusN).ToList();
                    NearbyMs1Scans.AddRange(AllScans.Where(c => c.MsOrder == 1 && c.ScanNumber < currentScanNumber).OrderBy(b => currentScanNumber - b.ScanNumber).Take(plusMinusN).ToList());
                }
                else if (Options.AveragingVector == AveragingVector.Before)
                {
                    NearbyMs1Scans = AllScans.Where(c => c.MsOrder == 1 && c.ScanNumber < currentScanNumber).OrderBy(b => currentScanNumber - b.ScanNumber).Take(plusMinusN).ToList();
                }
                else if (Options.AveragingVector == AveragingVector.After)
                {
                    NearbyMs1Scans = AllScans.Where(c => c.MsOrder == 1 && c.ScanNumber > currentScanNumber).OrderBy(b => b.ScanNumber - currentScanNumber).Take(plusMinusN).ToList();
                }

                Scan[] Ms1ScansCentroids = NearbyMs1Scans.ToArray();
                Run(Ms1ScansCentroids, PrecursorScan, scan, Options);
            }
        }

        /// <summary>
        /// Run a single Monocle scan.
        /// </summary>
        /// <param name="Ms1ScansCentroids"></param>
        /// <param name="ParentScan"></param>
        /// <param name="DependentScan"></param>
        public static void Run(Scan[] Ms1ScansCentroids, Scan ParentScan, Scan DependentScan, MonocleOptions Options)
        {
            double precursorMz = DependentScan.PrecursorMz; // This shold be precursorMz?
            int precursorCharge = DependentScan.PrecursorCharge;

            // For number of isotopes to consider
            int numIsotopes = 0;
            int monoisotopicIndex = 0;
            int numTheo = 4;
            int left = -7;

            IntRange charge_range = new IntRange(precursorCharge, precursorCharge);

            // For charge detection
            int best_charge = 0;
            double best_score = -1;
            int bestIndex = monoisotopicIndex;
            PeptideEnvelope envelope = new PeptideEnvelope(numIsotopes);
            if (Options.Charge_Detection)
            {
                charge_range = Options.Charge_Range_LowRes;
            }

            for(int charge_iterator = 0; charge_iterator < charge_range.High; charge_iterator++)
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

                envelope = PeptideEnvelopeExtractor.Extract(Ms1ScansCentroids, precursorMz, charge_iterator, left, numIsotopes);

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
                        }
                    }
                }
            } // end charge for loop

            if (best_charge > 0)
            {
                DependentScan.MonoisotopicCharge = best_charge;
            }

            double newMonoisotopicMz = CalculateMz(envelope.mzs[bestIndex], envelope.intensities[bestIndex]);

            newMonoisotopicMz = (newMonoisotopicMz == 0) ? precursorMz : newMonoisotopicMz;

            DependentScan.MonoisotopicMz = newMonoisotopicMz;

        } // Run

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
