using System;

namespace Monocle.Data
{
    /// <summary>
    /// for pulling spectral information from API scans
    /// </summary>
    public struct Centroid
    {
        public Centroid(double mz, float intensity)
        {
            Mz = mz;
            Intensity = intensity;
        }

        public double Mz;

        /// <summary>
        /// Centroid intensity
        /// </summary>
        public float Intensity;
    }
}
