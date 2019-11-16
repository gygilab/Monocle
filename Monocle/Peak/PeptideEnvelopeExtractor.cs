
using Monocle.Data;
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

            foreach (var mzs in output.mzs)
            {
                double sum = 0;
                int count = 0;
                foreach (var x in mzs)
                {
                    sum += x;
                    ++count;
                }
                double avg = 0;
                if (count > 0)
                {
                    avg = sum / count;
                }
                output.averageMz.Add(avg);
            }
            foreach (var intensities in output.intensities)
            {
                double sum = 0;
                int count = 0;
                foreach (var x in intensities)
                {
                    sum += x;
                    ++count;
                }
                double avg = 0;
                if (count > 0)
                {
                    avg = sum / count;
                }
                output.averageIntensity.Add(avg);
            }
            return output;
        }
    }

}