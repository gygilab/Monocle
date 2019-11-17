
using Monocle.Data;
using Monocle.Math;
using System.Collections.Generic;

namespace Monocle.Peak 
{
    public static class PeptideEnvelopeExtractor
    {
        const double AVERAGINE_DIFF = 1.00286864;

        public static PeptideEnvelope Extract(List<Scan> scans, double targetMz, int charge, int left, int numIsotopes)
        {
            List<double> isotopeWidths = new List<double>(new double[numIsotopes]);
            for (int i = left; i - left < numIsotopes; ++i)
            {
                isotopeWidths[i - left] = i * AVERAGINE_DIFF;
            }

            PeptideEnvelope output = new PeptideEnvelope(numIsotopes);
            foreach (Scan scan in scans)
            {
                if(scan != null)
                {
                    for (int i = 0; i < numIsotopes; ++i)
                    {
                        double matchMz = targetMz + (isotopeWidths[i] / charge);
                        int index = PeakMatcher.Match(scan, matchMz, 3, PeakMatcher.PPM);
                        if (index >= 0)
                        {
                            double mz = scan.Centroids[index].Mz;
                            double intensity = scan.Centroids[index].Intensity;
                            output.mzs[i].Add(mz);
                            output.intensities[i].Add(intensity);
                        }
                        else
                        {
                            output.mzs[i].Add(0);
                            output.intensities[i].Add(0);
                        }
                    }
                }
            }

            foreach (var intensities in output.intensities)
            {
                output.averageIntensity.Add(Vector.Average(intensities));
            }
            return output;
        }
    }

}