
using System;

namespace Monocle.Math 
{
    public static class Binomial
    {
        public static double P(int n, int k, double p)
        {
            return (NCr(n, k) * System.Math.Pow(p, k) * System.Math.Pow((1.0 - p), (n - k)));
        }

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