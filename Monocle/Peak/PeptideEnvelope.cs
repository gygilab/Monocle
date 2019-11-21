
using System.Collections.Generic;

namespace Monocle.Peak
{
    public class PeptideEnvelope
    {
        public List<List<double>> mzs;

        public List<List<double>> intensities;

        public List<double> averageIntensity;

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