
namespace Monocle.Peak {
    /// <summary>
    /// This class stores information about how many and which isotopes
    /// to consider within the isotopic envelope.
    /// </summary>
    class IsotopeRange {
        /// <summary>
        /// The total number of isotopes to consider.`
        /// </summary>
        public int Isotopes;

        /// <summary>
        /// The index offset of the original monoisotopic peak.
        /// </summary>
        public int MonoisotopicIndex;

        /// <summary>
        /// The number of alternate peaks to consider to the left 
        /// of the original monoisotopic index.
        /// </summary>
        public int Left;

        /// <summary>
        /// The number of isotopes to consider at a time during scoring.
        /// </summary>
        public int CompareSize;

        public IsotopeRange(double mass) {
            if (mass > 2900)
            {
                Isotopes = 14;
                Left = -6;
                CompareSize = 7;
            }
            else if (mass > 1200)
            {
                Isotopes = 10;
                Left = -4;
                CompareSize = 5;
            }
            else
            {
                Isotopes = 7;
                Left = -3;
                CompareSize = 4;
            }
            MonoisotopicIndex = -1 * Left;
        }
    }
}
