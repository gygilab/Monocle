
namespace Monocle.Data {
    public class Precursor {
        /// <summary>
        /// Constructor for Precursor
        /// </summary>
        /// <param name="mz"></param>
        public Precursor(double mz, double intensity=0, int charge=0)
        {
            Mz = mz;
            Intensity = intensity;
            Charge = charge;
        }
        public Precursor()
        {
        }
        /// <summary>
        /// The m/z of the precursor peak.
        /// This is often the (putative) monoisotopic m/z of the molecule.
        /// </summary>
        public double Mz { get; set; }

        /// <summary>
        /// Intensity of the precursor peak.
        /// </summary>
        public double Intensity { get; set; }

        /// <summary>
        /// Charge state of the precursor.
        /// </summary>
        public int Charge { get; set; }

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
        public double IsolationMz { get; set; }

        /// <summary>
        /// The size of the window that the instrument targeted for isolation.
        /// </summary>
        public double IsolationWidth { get; set; }

        /// <summary>
        /// Proportion of the intensity in the isolation window
        /// that belongs to the precursor.
        /// 
        /// This should be a value from zero to one.
        /// </summary>
        public double IsolationSpecificity { get; set; }
    }
}