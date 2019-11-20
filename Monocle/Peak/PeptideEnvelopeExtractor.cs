
using Monocle.Data;
using Monocle.Math;
using System.Collections.Generic;

namespace Monocle.Peak 
{
    public static class PeptideEnvelopeExtractor
    {
        /// <summary>
        /// Extract peaks that may be isotopes of the peak indicated by targetMz
        /// </summary>
        /// <param name="scans">Peaks will be extracted from this list of scans</param>
        /// <param name="targetMz">The m/z to start extraction</param>
        /// <param name="charge">The charge used to find neighboring isotopes</param>
        /// <param name="left">A negative or zero number to indicate the number of isotopes to extract to the left of targetMz</param>
        /// <param name="numIsotopes">The total number of isotopes to extract including the number indicated by "left"</param>
        /// <returns></returns>
        public static PeptideEnvelope Extract(List<Scan> scans, double targetMz, int charge, int left, int numIsotopes)
        {
            PeptideEnvelope output = new PeptideEnvelope(numIsotopes, scans.Count);
            foreach (Scan scan in scans)
            {
                for (int i = 0; i < numIsotopes; ++i)
                {
                    double matchMz = targetMz + (((i + left) * Mass.AVERAGINE_DIFF) / charge);
                    int index = PeakMatcher.Match(scan, matchMz, 3, PeakMatcher.PPM);
                    if (index >= 0)
                    {
                        double mz = scan.Centroids[index].Mz;
                        double intensity = scan.Centroids[index].Intensity;
                        output.mzs[i].Add(mz);
                        output.intensities[i].Add(intensity);
                    }
                }
            }

            int max = 0;
            foreach (var intensities in output.intensities)
            {
                if (intensities.Count > max)
                {
                    max = intensities.Count;
                }
                output.averageIntensity.Add(Vector.Average(intensities));
            }
            output.MaxPeakCount = max;
            return output;
        }

        public static void ScaleByPeakCount(List<double> x, PeptideEnvelope envelope, int i)
        {
            if(envelope.MaxPeakCount > 0)
            {
                for (int j = i; j < i + x.Count; ++j)
                {
                    x[j - i] *= envelope.mzs[j].Count / (double)envelope.MaxPeakCount;
                }
            }
        }
    }
}
