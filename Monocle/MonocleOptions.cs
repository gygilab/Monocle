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
        /// MS level at which monoisotopic m/z will be adjusted
        /// </summary>
        private int _MS_Level { get; set; } = 2;
        public int MS_Level
        {
            get
            {
                return _MS_Level;
            }
            set
            {
                if (value > 0 && value < 20)
                {
                    _MS_Level = value;
                }
            }
        }

        /// <summary>
        /// Toggle the use of charge detection, reports a single charge state
        /// </summary>
        public bool Charge_Detection { get; set; } = false;

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
        /// Polarity of the charges to be analyzed
        /// </summary>
        public Polarity Polarity { get; set; } = Polarity.Positive;

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

        /// <summary>
        /// avoid the monoisotopic peak detection.
        /// </summary>
        /// 
        /// <remarks>
        /// If this is true, precursors can still be modified
        /// if ForceCharges is true or if there are low-res MS1 scans.
        /// 
        /// ConvertOnly will also skip monoisotopic peak detection.
        /// </remarks>
        public bool SkipMono { get; set; } = false;

        /// <summary>
        /// Enable to assign the precursor information to MS3 scans from
        /// the parent MS2.
        /// </summary>
        public bool Ms2Ms3Precursor { get; set; } = false;

        /// <summary>
        /// If set to true, Monocle will look through the entire
        /// isolation window and re-assign the precursor m/z to the
        /// peak with the highest intensity before running monoisotopic
        /// peak detection
        /// </summary>
        public bool UseMostIntense { get; set; } = false;

        /// <summary>
        /// If set to true, Monocle will use the input monoisotopic m/z
        /// as a starting point.  Otherwise, it will start with the isolation m/z
        /// </summary>
        public bool RawMonoMz { get; set; } = false;

        /// <summary>
        /// Allow get/set of property based on property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object this[string propertyName]
        {
            get { return GetType().GetProperty(propertyName).GetValue(this); }
            set { GetType().GetProperty(propertyName).SetValue(this, value); }
        }
    }
}
