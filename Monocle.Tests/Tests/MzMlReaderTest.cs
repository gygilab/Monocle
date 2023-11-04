using Monocle.Data;
using Monocle.File;
using System;
using System.IO;
using System.Collections.Generic;
using Xunit;

namespace Monocle.Tests
{
    public class MzMlReaderTest
    {
        [Fact]
        public void TestOpen()
        {
            MzMlReader reader = new MzMlReader();
            reader.Open("data/small.pwiz.1.1.mzML", new ScanReaderOptions());
        }

        [Fact]
        public void TestReadScans() {
            MzMlReader reader = new MzMlReader();
            reader.Open("data/small.pwiz.1.1.mzML", new ScanReaderOptions());
            var scans = new List<Scan>();
            foreach (Scan scan in reader) {
                scans.Add(scan);
            }
            Assert.Equal(48, scans.Count);
            Assert.Equal(1, scans[0].ScanNumber);
            Assert.Equal(1, scans[0].MsOrder);
            Assert.Equal(19914, scans[0].PeakCount);
            Assert.Equal(19914, scans[0].Centroids.Count);
            Assert.Equal(200.000188, scans[0].Centroids[0].Mz, 6);
        }

        [Fact]
        public void TestReadMs2() {
            MzMlReader reader = new MzMlReader();
            reader.Open("data/small.pwiz.1.1.mzML", new ScanReaderOptions());
            var scans = new List<Scan>();
            foreach (Scan scan in reader) {
                scans.Add(scan);
            }
            var ms2Scan = scans[2];
            
            Assert.Equal(2, ms2Scan.MsOrder);
            Assert.Single(ms2Scan.Precursors);
            Assert.Equal(810.7899999, ms2Scan.Precursors[0].Mz, 3);
        }
    }
}
