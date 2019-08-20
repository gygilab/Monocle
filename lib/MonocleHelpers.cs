using System;
using System.Collections.Generic;

namespace MonocleUI
{
    public static class Pearson
    {
        public static double P(List<double> x, List<double> y)
        {

            double avgX = Pearson.Avg(x);
            double avgY = Pearson.Avg(y);
            double numerator = 0;
            double ex = 0; // Sum errors of x
            double ey = 0; // Sum errors of y
            for (int i = 0; i < x.Count; ++i)
            {
                numerator += (x[i] - avgX) * (y[i] - avgY);
                ex += (x[i] - avgX) * (x[i] - avgX);
                ey += (y[i] - avgY) * (y[i] - avgY);
            }
            if ((ex * ey) > 0)
            {
                return (numerator / Math.Pow(ex * ey, 0.5));
            }
            else
            {
                return 0;
            }
        }

        private static double Avg(List<double> x)
        {
            double sum = 0;
            int count = 0;
            foreach (var v in x)
            {
                sum += v;
                ++count;
            }
            return count > 0 ? sum / count : 0;
        }

    }

    public class PeptideEnvelope
    {
        public List<List<double>> mzs;

        public List<List<double>> intensities;

        public List<double> averageMz;

        public List<double> averageIntensity;

        public PeptideEnvelope(int numIsotopes)
        {
            this.mzs = new List<List<double>>();
            this.intensities = new List<List<double>>();
            for (int i = 0; i < numIsotopes; ++i)
            {
                this.mzs.Add(new List<double>());
                this.intensities.Add(new List<double>());
            }
            this.averageMz = new List<double>();
            this.averageIntensity = new List<double>();
        }
    }

    public static class Binomial
    {
        public static double P(int n, int k, double p)
        {
            return (NCr(n, k) * Math.Pow(p, k) * Math.Pow((1.0 - p), (n - k)));
        }

        private static double NCr(int n, int r)
        {
            double retVal = 1;
            for (int i = 1; i <= n; ++i)
            {
                retVal *= i;
                if (i <= r)
                {
                    retVal /= i;
                }
                if (i <= (n - r))
                {
                    retVal /= i;
                }
            }

            return retVal;
        }
    }

    public static class PeptideEnvelopeCalculator
    {
        /// <summary>
        /// Returns the intensity distribution of the isotopes a peptide based on the
        /// binomial probability of how many c13 are included.
        /// 
        /// The first element of the return value is zero - for comparisons
        /// we wouldnt expect a peak to the left.
        /// </summary>
        /// 
        /// <returns>The theoretical envelope.</returns>
        /// <param name="precursorMz">Precursor mz.</param>
        /// <param name="charge">Charge.</param>
        /// <param name="compareSize">The number of isotopes to consider</param>
        public static List<double> GetTheoreticalEnvelope(double precursorMz, int charge, int compareSize)
        {
            int numCarbons = EstimateCarbons(precursorMz, charge);
            List<double> output = new List<double>(new double[compareSize]);
            output[0] = 0.0;
            for (int i = 1; i < compareSize; ++i)
            {
                output[i] = Binomial.P(numCarbons, i - 1, 0.011);
            }

            return output;
        }

        /// <summary>
        /// Estimates the number of carbons in a peptide based only on its
		/// precursor m/z and charge.
        /// </summary>
		/// 
        /// <returns>Number of carbons</returns>
        /// <param name="mz">mz</param>
        /// <param name="charge">charge</param>
        private static int EstimateCarbons(double mz, int charge)
        {
            return (int)Math.Floor((((mz * charge) - (1.00728 * charge)) / 111) * 5.1);
        }

        /// <summary>
        /// Scales all values in the input so that the max is 1
        /// </summary>
		/// 
        /// <param name="x">The input list.</param>
        public static void Scale(List<double> x)
        {
            double max = 0;
            for (int j = 0; j < x.Count; ++j)
            {
                if (x[j] > max)
                {
                    max = x[j];
                }
            }
            if (max > 0)
            {
                for (int j = 0; j < x.Count; ++j)
                {
                    x[j] = x[j] / max;
                }
            }
        }

    }

    public static class PeptideEnvelopeExtractor
    {
        const double AVERAGINE_DIFF = 1.00286864;

        public static PeptideEnvelope Extract(Scan[] scans, double targetMz, int charge, int left, int numIsotopes)
        {
            List<double> isotopeWidths = new List<double>(new double[numIsotopes]);
            for (int i = left; i - left < numIsotopes; ++i)
            {
                isotopeWidths[i - left] = i * AVERAGINE_DIFF;
            }

            PeptideEnvelope output = new PeptideEnvelope(numIsotopes);
            foreach (Scan scan in scans)
            {
                if(scan != null)
                {
                    for (int i = 0; i < numIsotopes; ++i)
                    {
                        double matchMz = targetMz + (isotopeWidths[i] / charge);
                        int index = PeakMatcher.Match(scan, matchMz, 3, PeakMatcher.PPM);
                        if (index >= 0)
                        {
                            double mz = scan.Centroids[index].Mz;
                            double intensity = scan.Centroids[index].Intensity;
                            output.mzs[i].Add(mz);
                            output.intensities[i].Add(intensity);
                        }
                        else
                        {
                            output.mzs[i].Add(0);
                            output.intensities[i].Add(0);
                        }
                    }
                }
            }

            foreach (var mzs in output.mzs)
            {
                double sum = 0;
                int count = 0;
                foreach (var x in mzs)
                {
                    sum += x;
                    ++count;
                }
                double avg = 0;
                if (count > 0)
                {
                    avg = sum / count;
                }
                output.averageMz.Add(avg);
            }
            foreach (var intensities in output.intensities)
            {
                double sum = 0;
                int count = 0;
                foreach (var x in intensities)
                {
                    sum += x;
                    ++count;
                }
                double avg = 0;
                if (count > 0)
                {
                    avg = sum / count;
                }
                output.averageIntensity.Add(avg);
            }
            return output;
        }
    }

    public class PeakMatcher
    {
        public const int PPM = 1;
        public const int DALTON = 2;

        public static int Match(Scan scan, double targetMz, double tolerance, int tolUnits)
        {
            int i = NearestIndex(scan, targetMz);

            int count = scan.CentroidCount;
            bool foundNext = false;
            double errorNext = 0;
            if (i < count)
            {
                foundNext = true;
                errorNext = Math.Abs(scan.Centroids[i].Mz - targetMz);
            }

            bool foundPrevious = false;
            double errorPrevious = 0;
            if (i > 0)
            {
                foundPrevious = true;
                errorPrevious = Math.Abs(scan.Centroids[i - 1].Mz - targetMz);
            }

            if (!foundNext && !foundPrevious)
            {
                return -1;
            }

            if (!foundNext || (foundPrevious && errorPrevious < errorNext))
            {
                if (WithinError(targetMz, scan.Centroids[i - 1].Mz, tolerance, tolUnits))
                {
                    return i - 1;
                }
            }
            else if (!foundPrevious || (foundNext && errorNext < errorPrevious))
            {
                if (WithinError(targetMz, scan.Centroids[i].Mz, tolerance, tolUnits))
                {
                    return i;
                }
            }

            return -1;
        }

        private static int NearestIndex(Scan scan, double target)
        {
            int low = 0;
            int high = scan.CentroidCount;
            int mid = 0;

            while (true)
            {
                if (low == high)
                {
                    return low;
                }

                mid = (int)((high + low) * 0.5);

                if (scan.Centroids[mid].Mz < target)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid;
                }
            }
        }

        private static bool WithinError(double theoretical, double observed, double tolerance, int tolUnits)
        {
            switch (tolUnits)
            {
                case PeakMatcher.DALTON:
                    return Math.Abs(theoretical - observed) < tolerance;
                case PeakMatcher.PPM:
                    return Math.Abs(getPpm(theoretical, observed)) < tolerance;
                default:
                    break;
            }
            return false;
        }

        private static double getPpm(double theoretical, double observed)
        {
            return 1000000 * (theoretical - observed) / theoretical;
        }

    }
}
