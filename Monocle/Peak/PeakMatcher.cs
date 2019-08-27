
using Monocle.Data;
using System;

namespace Monocle.Peak 
{

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
                    return System.Math.Abs(theoretical - observed) < tolerance;
                case PeakMatcher.PPM:
                    return System.Math.Abs(getPpm(theoretical, observed)) < tolerance;
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