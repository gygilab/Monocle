
using System;
using System.Collections.Generic;

namespace Monocle.Math
{
    public static class Pearson
    {
        /// <summary>
        /// Calculate Pearson correlation from two lists
        /// </summary>
        public static double P(List<double> x, List<double> y)
        {
            double avgX = Vector.Average(x);
            double avgY = Vector.Average(y);
            double numerator = 0;
            double ex = 0; // Sum errors of x
            double ey = 0; // Sum errors of y
            for (int i = 0; i < x.Count; ++i)
            {
                numerator += (x[i] - avgX) * (y[i] - avgY);
                ex += (x[i] - avgX) * (x[i] - avgX);
                ey += (y[i] - avgY) * (y[i] - avgY);
            }
            if ((ex * ey) > 0)
            {
                return (numerator / System.Math.Pow(ex * ey, 0.5));
            }
            return 0;
        }
    }
}
