
using Monocle.Data;
using Monocle.File;
using System.Data.SQLite;
using Xunit;

namespace Monocle.Tests {
    public class MzDBWriterTest {

        private string path = "data/test.mzdb";

        public MzDBWriterTest() {
            var writer = new MzDBWriter();
            writer.Open(path);
            writer.WriteHeader(new ScanFileHeader());
            Scan scan1 = new Scan();
            scan1.ScanNumber = 1;
            scan1.BasePeakIntensity = 1;
            scan1.BasePeakMz = 1;
            scan1.FilterLine = "FOO";
            scan1.Centroids.Add(new Centroid(100, 200));
            scan1.Centroids.Add(new Centroid(110, 220));
            writer.WriteScan(scan1);

            Scan scan2 = new Scan();
            scan2.ScanNumber = 2;
            scan2.BasePeakIntensity = 123;
            scan2.BasePeakMz = 456;
            scan2.Centroids.Add(new Centroid(200, 300));
            scan2.Centroids.Add(new Centroid(210, 320));
            scan2.Precursors.Add(new Precursor(234, 345, 2));
            writer.WriteScan(scan2);
            writer.Close();
        }

        [Fact]
        public void testCreateDB() {
            Assert.True(System.IO.File.Exists(path));
        }

        [Fact]
        public void testVersion() {
            var db = new SQLiteConnection("Data Source=" + path);
            db.Open();
            string sql = "Select value from metadata where name='version'";
            var reader = new SQLiteCommand(sql, db).ExecuteReader();
            int valueCount = 0;
            while(reader.Read()) {
                Assert.Equal("1", reader.GetString(0));
                ++valueCount;
            }
            Assert.Equal(1, valueCount);
        }

        [Fact]
        public void testScan() {
            var db = new SQLiteConnection("Data Source=" + path);
            db.Open();

            string sql = "Select scan, filter_line, base_peak_mz, base_peak_intensity FROM scans WHERE scan=1";
            var reader = new SQLiteCommand(sql, db).ExecuteReader();
            int valueCount = 0;
            while(reader.Read()) {
                Assert.Equal(1, reader.GetInt32(0));
                Assert.Equal("FOO", reader.GetString(1));
                Assert.Equal(1, reader.GetDouble(2));
                Assert.Equal(1, reader.GetDouble(2));
                ++valueCount;
            }
            Assert.Equal(1, valueCount);
        }

        [Fact]
        public void testPeaks() {
            var reader = new MzDBReader();
            reader.Open(path, new ScanReaderOptions());
            foreach (Scan scan in reader) {
                if (scan.ScanNumber == 1) {
                    Assert.Equal("FOO", scan.FilterLine);
                    Assert.Equal(2, scan.Centroids.Count);
                    Assert.Equal(100, scan.Centroids[0].Mz);
                }
                else {
                    Assert.Equal(456, scan.BasePeakMz);
                    Assert.Single(scan.Precursors);
                    Assert.Equal(234, scan.Precursors[0].Mz);
                }
            }
        }
    }
}
