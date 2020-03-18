
using Monocle.Data;
using Monocle.Peak;
using System.Collections.Generic;
using Xunit;

namespace Monocle.Tests.Tests
{
    public class IsoSpecTest
    {
        [Fact]
        public void TestOutput()
        {
            double isolationMz = 1000.0;
            double precursorMz = 999.5;
            int charge = 2;
            double isolationWindow = 1.0;

            var peaks = new List<Centroid> {
                new Centroid { Mz = 800.0, Intensity = 100 },
                new Centroid { Mz = 999.5, Intensity = 100 },
                new Centroid { Mz = 999.9, Intensity = 100 },
                new Centroid { Mz = 1000.0, Intensity = 100 },
                new Centroid { Mz = 1000.1, Intensity = 100 },
                new Centroid { Mz = 1200.0, Intensity = 100 }
            };

            double output = IsolationSpecificityCalculator.calculate(peaks, isolationMz, precursorMz, charge, isolationWindow);
            Assert.Equal(0.5, output, 3);
        }
    }
}
