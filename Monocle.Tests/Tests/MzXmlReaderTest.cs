using Monocle.Data;
using Monocle.File;
using System;
using System.Collections.Generic;
using Xunit;

namespace Monocle.Tests
{
    public class MzXmlReaderTest
    {
        [Fact]
        public void TestOpen()
        {
            MzXmlReader reader = new MzXmlReader();
            reader.Open("data/orbixl-mini.mzxml");
        }

        [Fact]
        public void TestReadScans() {
            MzXmlReader reader = new MzXmlReader();
            reader.Open("data/orbixl-mini.mzxml");
            var scans = new List<Scan>();
            foreach (Scan scan in reader) {
                scans.Add(scan);
            }
            Assert.Equal(84, scans.Count);
            Assert.Equal(1, scans[0].ScanNumber);
            Assert.Equal(1, scans[0].MsOrder);
            Assert.Equal(1396, scans[0].PeakCount);
            Assert.Equal(1396, scans[0].Centroids.Count);
            Assert.Equal(356.43359375, scans[0].Centroids[0].Mz, 6);
        }

        [Fact]
        public void TestReadMs2() {
            MzXmlReader reader = new MzXmlReader();
            reader.Open("data/orbixl-mini.mzxml");
            var scans = new List<Scan>();
            foreach (Scan scan in reader) {
                scans.Add(scan);
            }
            var ms2Scan = scans[10];
            
            Assert.Equal(2, ms2Scan.MsOrder);
            Assert.Equal(1d, ms2Scan.Precursors.Count, 3);
            Assert.Equal(687.3921, ms2Scan.Precursors[0].Mz, 3);
        }
    }
}
