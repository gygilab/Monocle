
using System.Collections.Generic;
using System.Linq;
using Monocle.Data;

namespace Monocle.Peak {
    /// <summary>
    /// Return the proportion of intensity belonging to the peak given by
    /// mz and intensity within the window given by isolationWindow.
    /// </summary>
    public static class IsolationSpecificityCalculator {
        public static double calculate(List<Centroid> peaks, double mz, double intensity, double isolationWindow) {
            double halfIsolationWindow = isolationWindow / 2;

            List<Centroid> tempCentroid = new List<Centroid>();
            tempCentroid = peaks.Where(x => x.Mz <= mz + halfIsolationWindow && x.Mz >= mz - halfIsolationWindow).ToList();

            double isolationSpecificity = intensity / tempCentroid.Sum(cent => cent.Intensity);

            return isolationSpecificity;
        }
    }
}
