
using System.Collections.Generic;

namespace Monocle.Peak
{
    /// <summary>
    /// Class to track peptide envelopes across one or more scans
    /// </summary>
    public class PeptideEnvelope
    {
        /// <summary>
        /// Parent list for each isotope, child list for scan mz values
        /// </summary>
        public List<List<double>> mzs;
        /// <summary>
        /// Parent list for each isotope, child list for scan intensity values
        /// </summary>
        public List<List<double>> intensities;
        /// <summary>
        /// Average intensity of each isotope peak across scans
        /// </summary>
        public List<double> averageIntensity;
        /// <summary>
        /// The maximum number of peaks for the peptide envelope
        /// </summary>
        public int MaxPeakCount;

        public PeptideEnvelope(int numIsotopes, int reserve)
        {
            mzs = new List<List<double>>(numIsotopes);
            intensities = new List<List<double>>(numIsotopes);
            for (int i = 0; i < numIsotopes; ++i)
            {
                mzs.Add(new List<double>(reserve));
                intensities.Add(new List<double>(reserve));
            }
            averageIntensity = new List<double>(reserve);
        }
    }
}