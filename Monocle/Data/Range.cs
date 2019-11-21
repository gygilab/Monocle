
namespace Monocle.Data
{
    /// <summary>
    /// Class to handle a charge range
    /// </summary>
    public class ChargeRange
    {
        public int High = 6;

        public int Low = 2;

        public Polarity Polarity = Polarity.Positive;

        public ChargeRange(string range = "2:6")
        {
            if(range == null) {
                return;
            }
            string[] args = range.Split(':');
            if (args.Length < 2) {
                return;
            }
            int tempLow = int.Parse(args[0]);
            int tempHigh = int.Parse(args[1]);
            Polarity = (tempLow > 0) ? Polarity.Positive : Polarity.Negative;
            Low = (Polarity == Polarity.Positive) ? tempLow : tempLow * -1;
            High = (Polarity == Polarity.Positive) ? tempHigh : tempHigh * -1;
        }

        public ChargeRange(int low, int high, Polarity polarity = Polarity.Positive)
        {
            Low = (polarity == Polarity.Positive) ? low : low * -1;
            High = (polarity == Polarity.Positive) ? high : high * -1;
            Polarity = polarity;
        }
    }
}
