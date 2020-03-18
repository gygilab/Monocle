
using Monocle.Data;
using System.Collections.Generic;

namespace Monocle.Peak {
    /// <summary>
    /// Return the proportion of intensity belonging to the peak given by
    /// mz and intensity within the window given by isolationWindow.
    /// </summary>
    public static class IsolationSpecificityCalculator {
        public static double calculate(List<Centroid> peaks, double isolationMz, double precursorMz, int charge, double isolationWindow) {
            if (peaks.Count == 0) {
                return 0;
            }

            double precursorIntensity = 0;
            double totalIntensity = 0;
            double lowMz = isolationMz - (isolationWindow / 2.0);
            double highMz = isolationMz + (isolationWindow / 2.0);

            int i = PeakMatcher.NearestIndex(peaks, lowMz);
            if (peaks[i].Mz < lowMz) {
                ++i;
            }
            for ( ; i < peaks.Count && peaks[i].Mz < highMz; ++i) {
                var peak = peaks[i];
                
                // if the peak is within 20 ppm of any isotope
                bool isPrecursor = false;
                for (int j = -6; j < 7; ++j) {
                    double theoreticalMass = precursorMz + (j * (Mass.AVERAGINE_DIFF / charge));
                    if (System.Math.Abs(PeakMatcher.getPpm(theoreticalMass, peak.Mz)) < 20) {
                        isPrecursor = true;
                        break;
                    }
                }

                if (isPrecursor) {
                    precursorIntensity += peak.Intensity;
                }
                totalIntensity += peak.Intensity;
            }

            if(totalIntensity < 1) {
                return 0;
            }
            return precursorIntensity / totalIntensity;
        }
    }
}
