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
}
