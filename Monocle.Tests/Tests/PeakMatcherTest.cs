
using Monocle.Data;
using Monocle.Peak;
using System.Collections.Generic;
using Xunit;

namespace Monocle.Tests
{
    public class PeakMatcherTest
    {
        [Fact]
        public void NearestTest()
        {
            double targetMz = 999.99;
            var peaks = GetTestPeaks();
            int i = PeakMatcher.NearestIndex(peaks, targetMz);
            Assert.Equal(3, i);
        }

        [Fact]
        public void NearestEndTest()
        {
            double targetMz = 1200.1;
            var peaks = GetTestPeaks();
            int i = PeakMatcher.NearestIndex(peaks, targetMz);
            Assert.Equal(5, i);
        }

        [Fact]
        public void NearestStartTest()
        {
            double targetMz = 1;
            var peaks = GetTestPeaks();
            int i = PeakMatcher.NearestIndex(peaks, targetMz);
            Assert.Equal(0, i);
        }

        [Fact]
        public void MatchTest() {
            double targetMz = 999.9999;
            double tolerance = 10;
            var units = PeakMatcher.PPM;
            var scan = GetTestScan();
            int i = PeakMatcher.Match(scan, targetMz, tolerance, units);
            Assert.Equal(3, i);
        }

        [Fact]
        public void NoMatchTest() {
            double targetMz = 400;
            double tolerance = 10;
            var units = PeakMatcher.PPM;
            var scan = GetTestScan();
            int i = PeakMatcher.Match(scan, targetMz, tolerance, units);
            Assert.Equal(-1, i);
        }

        [Fact]
        public void MostIntenseTest() {
            double targetMz = 980;
            double tolerance = 30;
            var units = PeakMatcher.DALTON;
            var scan = GetTestScan();
            int i = PeakMatcher.MostIntenseIndex(scan, targetMz, tolerance, units);
            Assert.Equal(3, i);

            targetMz = 400;
            tolerance = 10;
            i = PeakMatcher.MostIntenseIndex(scan, targetMz, tolerance, units);
            Assert.Equal(-1, i);

            targetMz = 1000;
            tolerance = 300;
            i = PeakMatcher.MostIntenseIndex(scan, targetMz, tolerance, units);
            Assert.Equal(0, i);
        }

        [Fact]
        public void EmptyTest() {
            var scan = new List<Centroid>();
            int i = PeakMatcher.NearestIndex(scan, 1000.0);
            Assert.Equal(0, i);
        }

        private List<Centroid> GetTestPeaks() {
            var peaks = new List<Centroid> {
                new Centroid { Mz = 800.0, Intensity = 200 },
                new Centroid { Mz = 999.5, Intensity = 100 },
                new Centroid { Mz = 999.9, Intensity = 100 },
                new Centroid { Mz = 1000.0, Intensity = 150 },
                new Centroid { Mz = 1000.1, Intensity = 100 },
                new Centroid { Mz = 1200.0, Intensity = 100 }
            };
            return peaks;
        }

        private Scan GetTestScan() {
            var scan = new Scan();
            scan.Centroids = GetTestPeaks();
            scan.PeakCount = scan.Centroids.Count;
            return scan;
        }
    }
}
