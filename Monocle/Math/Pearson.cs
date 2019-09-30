
using System;
using System.Collections.Generic;

namespace Monocle.Math
{
    public static class Pearson
    {
        /// <summary>
        /// Calculate Pearson correlation from two lists
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double P(List<double> x, List<double> y)
        {

            double avgX = Avg(x);
            double avgY = Avg(y);
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
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calculate average value
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double Avg(List<double> x)
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