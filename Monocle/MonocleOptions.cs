
using Monocle.Data;

namespace Monocle
{
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
        public int Number_Of_Scans_To_Average { get; set; } = 12;

        public AveragingVector AveragingVector { get; set; } = AveragingVector.Both;

        public bool Charge_Detection { get; set; } = false;

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
        public ChargeRange Charge_Range { get; set; } = new ChargeRange(2, 6);

        public ChargeRange ChargeRangeUnknown { get; set; } = new ChargeRange(2, 3);

        public bool ForceCharges { get; set; } = false;

        public bool WriteDebugString { get; set; } = false;

        public bool WriteSps { get; set; } = false;

        public OutputFileType OutputFileType { get; set; } = OutputFileType.mzxml;

        public bool ConvertOnly = false;
    }
}