
using System.Collections.Generic;

namespace Monocle.Math {
    public static class Vector {
        /// <summary>
        /// Calculate the dot product of two lists.
        /// </summary>
        public static double Dot(List<double> a, List<double> b) {
            double result = 0;
            for(int i = 0; i < a.Count && i < b.Count; ++i) {
                result += a[i] * b[i];
            }
            return result;
        }

        /// <summary>
        /// Calculate average value
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Average(List<double> x)
        {
            double sum = 0;
            int count = 0;
            foreach (var v in x)
            {
                sum += v;
                ++count;
            }
            return count > 0 ? sum / count : 0;
        }
    }
}
