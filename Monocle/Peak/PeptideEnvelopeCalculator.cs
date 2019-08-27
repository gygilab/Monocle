
using Monocle.Math;
using System.Collections.Generic;

namespace Monocle.Peak
{
    public static class PeptideEnvelopeCalculator
    {
        /// <summary>
        /// Returns the intensity distribution of the isotopes a peptide based on the
        /// binomial probability of how many c13 are included.
        /// 
        /// The first element of the return value is zero - for comparisons
        /// we wouldnt expect a peak to the left.
        /// </summary>
        /// 
        /// <returns>The theoretical envelope.</returns>
        /// <param name="precursorMz">Precursor mz.</param>
        /// <param name="charge">Charge.</param>
        /// <param name="compareSize">The number of isotopes to consider</param>
        public static List<double> GetTheoreticalEnvelope(double precursorMz, int charge, int compareSize)
        {
            int numCarbons = EstimateCarbons(precursorMz, charge);
            List<double> output = new List<double>(new double[compareSize]);
            output[0] = 0.0;
            for (int i = 1; i < compareSize; ++i)
            {
                output[i] = Binomial.P(numCarbons, i - 1, 0.011);
            }

            return output;
        }

        /// <summary>
        /// Estimates the number of carbons in a peptide based only on its
		/// precursor m/z and charge.
        /// </summary>
		/// 
        /// <returns>Number of carbons</returns>
        /// <param name="mz">mz</param>
        /// <param name="charge">charge</param>
        private static int EstimateCarbons(double mz, int charge)
        {
            return (int)System.Math.Floor((((mz * charge) - (1.00728 * charge)) / 111) * 5.1);
        }

        /// <summary>
        /// Scales all values in the input so that the max is 1
        /// </summary>
		/// 
        /// <param name="x">The input list.</param>
        public static void Scale(List<double> x)
        {
            double max = 0;
            for (int j = 0; j < x.Count; ++j)
            {
                if (x[j] > max)
                {
                    max = x[j];
                }
            }
            if (max > 0)
            {
                for (int j = 0; j < x.Count; ++j)
                {
                    x[j] = x[j] / max;
                }
            }
        }

    }
}