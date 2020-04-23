
using Monocle.Data;
using System.Collections.Generic;

namespace Monocle.Peak 
{

    public class PeakMatcher
    {
        public const int PPM = 1;
        public const int DALTON = 2;

        public static int Match(Scan scan, double targetMz, double tolerance, int tolUnits)
        {
            int i = NearestIndex(scan.Centroids, targetMz);

            int count = scan.PeakCount;
            bool foundNext = false;
            double errorNext = 0;
            if (i < count)
            {
                foundNext = true;
                errorNext = System.Math.Abs(scan.Centroids[i].Mz - targetMz);
            }

            bool foundPrevious = false;
            double errorPrevious = 0;
            if (i > 0)
            {
                foundPrevious = true;
                errorPrevious = System.Math.Abs(scan.Centroids[i - 1].Mz - targetMz);
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

        /// <summary>
        /// Returns the index of the most intense peak in the window.
        /// Returns -1 if none is found.
        /// </summary>
        /// <param name="scan">The scan that will be searched</param>
        /// <param name="targetMz">The center of the window where peaks will be considered</param>
        /// <param name="tolerance">window will be +/- this value</param>
        /// <param name="tolUnits">Units can be DALTON or PPM</param>
        /// <returns></returns>
        public static int MostIntenseIndex(Scan scan, double targetMz, double tolerance, int tolUnits)
        {
            double lowMz = targetMz - tolerance;
            double highMz = targetMz + tolerance;
            if (tolUnits == PPM) {
                double delta = tolerance * targetMz / 1000000;
                lowMz = targetMz - delta;
                highMz = targetMz + delta;
            }
            var peaks = scan.Centroids;
            int i = PeakMatcher.NearestIndex(peaks, lowMz);
            if (peaks[i].Mz < lowMz) {
                ++i;
            }
            double maxIntensity = 0;
            int bestIndex = -1;
            for ( ; i < peaks.Count && peaks[i].Mz < highMz; ++i) {
                if (peaks[i].Intensity > maxIntensity) {
                    maxIntensity = peaks[i].Intensity;
                    bestIndex = i;
                }
            }
            return bestIndex;
        }

        public static int NearestIndex(List<Centroid> peaks, double target)
        {
            int low = 0;
            int high = peaks.Count - 1;
            int mid = 0;

            while (true)
            {
                if (low == high)
                {
                    return low;
                }

                mid = (int)((high + low) * 0.5);

                if (peaks[mid].Mz < target)
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
                    return System.Math.Abs(theoretical - observed) < tolerance;
                case PeakMatcher.PPM:
                    return System.Math.Abs(getPpm(theoretical, observed)) < tolerance;
                default:
                    break;
            }
            return false;
        }

        public static double getPpm(double theoretical, double observed)
        {
            return 1000000 * (theoretical - observed) / theoretical;
        }

    }
}