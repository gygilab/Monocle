using Monocle.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Monocle.Tests.Tests
{
    public class FileTest
    {
        List<Data.Scan> Scans { get; set; } = new List<Data.Scan>();

        [Fact]
        public void Mzxml()
        {
            string fileName = "orbixl-mini.mzxml";
            string newFile = Directory.GetCurrentDirectory() + "data/" + fileName;
            MZXML.Consume(newFile, Scans);
            // Verify that the mzXML can be read and that it contains 84 scans
            Assert.True(Scans.Count == 84);
        }

        [Fact]
        public void Raw()
        {

        }

        [Fact]
        public void Csv()
        {

        }
    }
}
