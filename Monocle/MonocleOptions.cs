
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

        /// <summary>
        /// Default to charges 2 - 6.
        /// </summary>
        public ChargeRange Charge_Range { get; set; } = new ChargeRange(2, 6);
    }
}