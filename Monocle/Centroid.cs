using System;

namespace Monocle
{
    /// <summary>
    /// for pulling spectral information from API scans
    /// </summary>
    public class Centroid : IComparable
    {
        public Centroid()
        {

        }

        public Centroid(double mz, double intensity)
        {
            Mz = mz;
            Intensity = intensity;
        }

        private double mz = 500;
        public double Mz
        {
            get
            {
                return mz;
            }
            set
            {
                iBin = Bin.AssignBin(value);
                mz = value;
            }
        }
        public double Precursor { get; set; } = 500;
        public double Intensity { get; set; } = 0;
        public double Noise { get; set; } = 0;
        public int iBin { get; private set; } = 0;
        public double IsolationSpecificity { get; set; } = 0;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is Centroid otherCentroid)
            {
                return Intensity.CompareTo(otherCentroid.Intensity);
            }
            else
            {
                throw new ArgumentException("Object is not a Centroid");
            }
        }
    }

    /// <summary>
    /// Bin m/z values based on the Comet binning approach
    /// </summary>
    public static class Bin
    {
        public static double fragment_bin_tol { get; set; } = 1;
        public static double fragment_bin_offset { get; set; } = 0.4;
        public static double dInverseBinWidth { get; } = 1 / fragment_bin_tol;
        public static double dOneMinusBinOffset { get; } = 1.0 - fragment_bin_offset;

        public static int AssignBin(double dMass)
        {
            return (int)(dMass * dInverseBinWidth + dOneMinusBinOffset);
        }
    }
}
