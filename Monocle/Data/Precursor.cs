
namespace Monocle.Data {
    public class Precursor {
        /// <summary>
        /// The m/z of the precursor peak.
        /// This is often the (putative) monoisotopic m/z of the molecule.
        /// </summary>
        public double Mz;

        /// <summary>
        /// Intensity of the precursor peak.
        /// </summary>
        public double Intensity;

        /// <summary>
        /// Charge state of the precursor.
        /// </summary>
        public int Charge;

        /// <summary>
        /// Precursor M+H
        /// </summary>
        public double Mh { get
            {
                return (Mz * Charge) - (Mass.ProtonMass * (Charge - 1));
            }
        }

        /// <summary>
        /// The m/z that the instrument targeted for isolation.
        /// </summary>
        public double IsolationMz;

        /// <summary>
        /// The size of the window that the instrument targeted for isolation.
        /// </summary>
        public double IsolationWidth;

        /// <summary>
        /// Proportion of the intensity in the isolation window
        /// that belongs to the precursor.
        /// 
        /// This should be a value from zero to one.
        /// </summary>
        public double IsolationSpecificity;
    }
}