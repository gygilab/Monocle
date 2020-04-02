using Monocle.Data;

namespace Monocle
{
    /// <summary>
    /// Indicate how to average precursor peaks
    /// </summary>
    public enum AveragingVector
    {
        Before,
        After,
        Both
    }

    /// <summary>
    /// Flexible options stack to run monocle
    /// </summary>
    public class MonocleOptions
    {
        /// <summary>
        /// The number of scans to average, default: +/- 6
        /// </summary>
        public int Number_Of_Scans_To_Average { get; set; } = 12;

        /// <summary>
        /// Toggle to average scans before or after the current Precursor scan, or both
        /// </summary>
        public AveragingVector AveragingVector { get; set; } = AveragingVector.Both;

        /// <summary>
        /// Toggle the use of charge detection, reports a single charge state
        /// </summary>
        public bool Charge_Detection { get; set; } = false;

        /// <summary>
        /// MS level at which monoisotopic m/z will be adjusted
        /// </summary>
        private int _MS_Level { get; set; } = 2;
        public int MS_Level { get
            {
                return _MS_Level;
            }
            set
            {
                if(value > 0 && value < 20)
                {
                    _MS_Level = value;
                }
            }
        }

        /// <summary>
        /// Default to charges 2 - 6.
        /// </summary>
        public Polarity Polarity { get; set; } = Polarity.Positive;

        /// <summary>
        /// Default to charges 2 - 6.
        /// </summary>
        public ChargeRange Charge_Range { get; set; } = new ChargeRange(2, 6);

        /// <summary>
        /// Set the charge range for peaks with unknown charge state
        /// </summary>
        public ChargeRange ChargeRangeUnknown { get; set; } = new ChargeRange(2, 3);

        /// <summary>
        /// Output multiple precursors with charges even if charge is known.
        /// </summary>
        public bool ForceCharges { get; set; } = false;

        /// <summary>
        /// Verbose debug output.
        /// </summary>
        public bool WriteDebugString { get; set; } = false;

        /// <summary>
        /// Write SPS ions as independent precursors.
        /// </summary>
        public bool WriteSps { get; set; } = false;

        /// <summary>
        /// Choose to output an mzXML "mzxml" or CSV file "csv".
        /// </summary>
        public OutputFileType OutputFileType { get; set; } = OutputFileType.mzxml;

        /// <summary>
        /// Write output file without modifying precursors.
        /// </summary>
        public bool ConvertOnly { get; set; } = false;
    }
}