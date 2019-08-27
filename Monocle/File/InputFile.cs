using Monocle.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocle.File
{
    public class InputFile
    {
        public InputFile()
        {
            Ms1ScansCentroids = new Scan[Num_Ms1_Scans_To_Average];
        }

        public static Scan[] Ms1ScansCentroids { get; set; }

        public static int Num_Ms1_Scans_To_Average { get; set; } = 6;
        public static bool DetectCharge { get; set; } = true;
        public static DoubleRange Charge_Range { get; set; } = new DoubleRange(2, 6);
        
        public static Scan ParentScan = new Scan();
        public static string ParentFile { get; set; } = "";
        private static int _Ms1ScanIndex { get; set; } = 0;
        public static int Ms1ScanIndex
        {
            get
            {
                return _Ms1ScanIndex;
            }
            set
            {
                if (value >= Ms1ScansCentroids.Length)
                {
                    _Ms1ScanIndex = 0;
                }
                else
                {
                    _Ms1ScanIndex = value;
                }
            }
        }

    }
}
