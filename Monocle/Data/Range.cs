using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocle.Data
{
    public struct DoubleRange
    {
        public double High { get; set; }
        public double Low { get; set; }

        public DoubleRange(double low, double high)
        {
            Low = low;
            High = high;
        }

        public DoubleRange(int low, int high)
        {
            Low = (double)low;
            High = (double)high;
        }
    }

    public struct IntRange
    {
        public int High { get; set; }
        public int Low { get; set; }

        public bool IsSet { get; }

        public IntRange(int low, int high)
        {
            Low = low;
            High = high;
            IsSet = true;
        }

        public IntRange(double low, double high)
        {
            Low = (int)low;
            High = (int)high;
            IsSet = true;
        }
    }

    public class ChargeRange
    {
        private int _Low { get; set; } = 2;
        private int _High { get; set; } = 6; 
        public int High { get; set; } = 6;
        public int Low { get; set; } = 2;

        public Polarity Polarity { get; set; } = Polarity.Positive;

        public bool IsSet { get; } = false;

        public ChargeRange(string cli_arg = "2:6")
        {
            string[] args = cli_arg.Split(':');
            int tempLow = int.Parse(args[0]);
            int tempHigh = int.Parse(args[1]);
            Polarity = (tempLow > 0) ? Polarity.Positive : Polarity.Negative;
            Low = (Polarity == Polarity.Positive) ? tempLow : tempLow * -1;
            High = (Polarity == Polarity.Positive) ? tempHigh : tempHigh * -1;
            IsSet = true;
        }

        public ChargeRange(int low, int high, Polarity polarity = Polarity.Positive)
        {
            Low = (polarity == Polarity.Positive) ? low : low * -1;
            High = (polarity == Polarity.Positive) ? high : high * -1;
            Polarity = polarity;
            IsSet = true;
        }

        public ChargeRange(double low, double high, Polarity polarity = Polarity.Positive)
        {
            Low = (polarity == Polarity.Positive) ? (int)low : (int)low * -1;
            High = (polarity == Polarity.Positive) ? (int)high : (int)high * -1;
            Polarity = polarity;
            IsSet = true;
        }
    }

    public enum Polarity
    {
        Positive,
        Negative,
        None
    }
}
