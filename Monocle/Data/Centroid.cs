using System;

namespace Monocle.Data
{
    /// <summary>
    /// for pulling spectral information from API scans
    /// </summary>
    public struct Centroid
    {
        public Centroid(double mz, double intensity, double baseline=0, double noise=0, double purity = 0, uint resolution = 0)
        {
            Mz = mz;
            Intensity = intensity;
            Baseline = baseline;
            Noise = noise;
            Purity = purity;
            Resolution = resolution;
        }

        /// <summary>
        /// Centroid m/z
        /// </summary>
        public double Mz { get; set; }

        /// <summary>
        /// Baseline
        /// </summary>
        public double Baseline { get; set; }

        /// <summary>
        /// Centroid intensity
        /// </summary>
        public double Intensity { get; set; }

        /// <summary>
        /// Source noise level
        /// </summary>
        public double Noise { get; set; }

        /// <summary>
        /// Purity of the centroid in the scan
        /// </summary>
        public double Purity { get; set; }

        /// <summary>
        /// Resolution of the Peak
        /// </summary>
        public uint Resolution { get; set; }
    }
}
