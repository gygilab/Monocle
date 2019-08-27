
using System.Collections.Generic;

namespace Monocle.Peak
{
    public class PeptideEnvelope
    {
        public List<List<double>> mzs;

        public List<List<double>> intensities;

        public List<double> averageMz;

        public List<double> averageIntensity;

        public PeptideEnvelope(int numIsotopes)
        {
            mzs = new List<List<double>>();
            intensities = new List<List<double>>();
            for (int i = 0; i < numIsotopes; ++i)
            {
                mzs.Add(new List<double>());
                intensities.Add(new List<double>());
            }
            averageMz = new List<double>();
            averageIntensity = new List<double>();
        }
    }
}