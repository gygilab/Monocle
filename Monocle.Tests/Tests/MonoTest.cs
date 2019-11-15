using System;
using System.Collections.Generic;
using Monocle;
using Monocle.Data;
using Monocle.File;
using Xunit;

namespace Monocle.Tests.Tests
{
    public class MonoTest
    {
        // d00810.raw scan 2220
        // Corrent mono m/z is 687.39195
        // same as isolation m/z
        [Fact]
        public void Mono1()
        {
            MzXmlReader reader = new MzXmlReader();
            reader.Open("data/orbixl-mini.mzxml");
            var scans = new List<Scan>();
            Scan ms2Scan = new Scan();
            Scan parentScan = new Scan();
            GetBlock(reader, 1, ref ms2Scan, ref parentScan, ref scans);
            MonocleOptions options = new MonocleOptions();
            Monocle.Run(scans.ToArray(), parentScan, ms2Scan, options);
            Assert.Equal(687.39195, ms2Scan.MonoisotopicMz, 3);
        }
        
        // d00810.raw scan 4734
        // Correct mono m/z is 1009.98842
        [Fact]
        public void Mono2()
        {
            MzXmlReader reader = new MzXmlReader();
            reader.Open("data/orbixl-mini.mzxml");
            var scans = new List<Scan>();
            Scan ms2Scan = new Scan();
            Scan parentScan = new Scan();
            GetBlock(reader, 2, ref ms2Scan, ref parentScan, ref scans);
            MonocleOptions options = new MonocleOptions();
            Monocle.Run(scans.ToArray(), parentScan, ms2Scan, options);
            Assert.Equal(1009.98842, ms2Scan.MonoisotopicMz, 2);
        }

        // d00810.raw scan 5020
        // Correct mono m/z is 869.449817
        [Fact]
        public void Mono3()
        {
            MzXmlReader reader = new MzXmlReader();
            reader.Open("data/orbixl-mini.mzxml");
            var scans = new List<Scan>();
            Scan ms2Scan = new Scan();
            Scan parentScan = new Scan();
            GetBlock(reader, 3, ref ms2Scan, ref parentScan, ref scans);
            MonocleOptions options = new MonocleOptions();
            Monocle.Run(scans.ToArray(), parentScan, ms2Scan, options);
            Assert.Equal(869.449817, ms2Scan.MonoisotopicMz, 3);
        }

        // The test mzxml contains 4 ms2 scans with +/- 10 ms1 scans 
        // around each. This method reads one ms2 scan and its ms1 scans.
        private void GetBlock(MzXmlReader reader, int block, ref Scan ms2Scan, ref Scan parentScan, ref List<Scan> ms1Scans) {
            int skipCount = (block - 1) * 21;
            Scan lastMs1 = null;
            foreach(Scan scan in reader) {
                if (skipCount > 0) {
                    --skipCount;
                    continue;
                }
                if (scan.MsOrder == 1) {
                    ms1Scans.Add(scan);
                    lastMs1 = scan;
                }
                else if(scan.MsOrder == 2) {
                    ms2Scan = scan;
                    parentScan = lastMs1;
                }
                if (ms1Scans.Count >= 20) {
                    break;
                }
            }
        }
    }
}
