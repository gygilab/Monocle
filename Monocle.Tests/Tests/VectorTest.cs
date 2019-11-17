
using System.Collections.Generic;
using Monocle.Math;
using Xunit;

namespace Monocle.Tests {
    public class VectorTest {
        [Fact]
        public void TestAverage() {
            Assert.Equal(2, Vector.Average(new List<double>{1, 2, 3}));
            Assert.Equal(0, Vector.Average(new List<double>()));
            Assert.Equal(2.5, Vector.Average(new List<double>{2, 3}));
        }

        [Fact]
        public void TestDot() {
            Assert.Equal(0.75, Vector.Dot(
                new List<double>{1, 1, 1},
                new List<double>{0.25, 0.25, 0.25}
            ));
            Assert.Equal(0, Vector.Dot(
                new List<double>{1, 1, 1},
                new List<double>()
            ));
        }

        [Fact]
        public void TestWeightedAverage() {
            Assert.Equal(2.4, Vector.WeightedAverage(
                new List<double>{1, 2, 3},
                new List<double>{1, 1, 3}
            ));
            Assert.Equal(0, Vector.WeightedAverage(
                new List<double>{1, 1, 1},
                new List<double>()
            ));
        }
    }
}