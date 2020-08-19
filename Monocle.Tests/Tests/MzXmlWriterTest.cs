
using System.IO;
using Monocle.Data;
using Monocle.File;
using Xunit;

namespace Monocle.Tests
{
    public class MzXmlWriterTest
    {
        [Fact]
        public void testSanity() {
            Assert.Equal(1, 1);
        }

        [Fact]
        public void testOutput() {
            var reader = new MzXmlReader();
            reader.Open("data/orbixl-mini.mzxml");

            string dir = Directory.GetCurrentDirectory();

            var writer = new MzXmlWriter();
            writer.Open("data/mzxml-writer-test.mzxml");

            var header = new ScanFileHeader();
            header.FileName = "mzxml-writer-test.mzxml";
            header.FilePath = "data/mzxml-writer-test.mzxml";
            writer.WriteHeader(header);

            var scans = reader.GetEnumerator();
            scans.MoveNext();
            Scan scan1 = (Scan)scans.Current;
            scans.MoveNext();
            Scan scan2 = (Scan)scans.Current;

            writer.WriteScan(scan1);
            writer.WriteScan(scan2);
            writer.Close();

            reader = new MzXmlReader();
            reader.Open("data/mzxml-writer-test.mzxml");
            
            scans = reader.GetEnumerator();
            scans.MoveNext();
            Scan scan3 = (Scan)scans.Current;
            scans.MoveNext();
            Scan scan4 = (Scan)scans.Current;

            Assert.Equal(scan1.ScanNumber, scan3.ScanNumber);
            Assert.Equal(scan1.RetentionTime, scan3.RetentionTime);
            Assert.Equal(scan1.PeakCount, scan3.PeakCount);
            Assert.Equal(scan1.Centroids[0].Mz, scan3.Centroids[0].Mz);
            Assert.Equal(scan1.Centroids[11].Intensity, scan3.Centroids[11].Intensity);
        }
    }
}