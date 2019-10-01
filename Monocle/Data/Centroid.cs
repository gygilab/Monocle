using System;

namespace Monocle.Data
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
                mz = value;
            }
        }
        /// <summary>
        /// Precursor m/z
        /// </summary>
        public double Precursor { get; set; } = 500;
        /// <summary>
        /// Centroid intensity
        /// </summary>
        public double Intensity { get; set; } = 0;
        /// <summary>
        /// Centroid Noise
        /// </summary>
        public double Noise { get; set; } = 0;
        /// <summary>
        /// Bin that the centroid exists in
        /// </summary>
        public int iBin { get; private set; } = 0;
        /// <summary>
        /// Centroid isolation purity, 0-1.
        /// </summary>
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
}
