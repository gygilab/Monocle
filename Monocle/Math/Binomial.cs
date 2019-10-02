
using System;

namespace Monocle.Math 
{
    public static class Binomial
    {
        /// <summary>
        /// Binomial probability mass function
        /// </summary>
        /// <param name="n">trials</param>
        /// <param name="k">successes</param>
        /// <param name="p">probability (e.g. 1.1% for carbon C13)</param>
        /// <returns></returns>
        public static double P(int n, int k, double p)
        {
            return (NCr(n, k) * System.Math.Pow(p, k) * System.Math.Pow((1.0 - p), (n - k)));
        }

        /// <summary>
        /// Calculate binomial coefficient
        /// </summary>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private static double NCr(int n, int r)
        {
            double retVal = 1;
            for (int i = 1; i <= n; ++i)
            {
                retVal *= i;
                if (i <= r)
                {
                    retVal /= i;
                }
                if (i <= (n - r))
                {
                    retVal /= i;
                }
            }

            return retVal;
        }
    }
}