using Monocle.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Monocle.File
{
    public static class FlatScanExtension
    {
        public static string ScanToMonocleString(this Scan scan, string delimiter = ",")
        {

            return scan.ScanNumber + delimiter + //scan number
                scan.MonoisotopicMz + delimiter + //precursor m/z
                scan.MonoisotopicMH + delimiter + //precursor M+H
                scan.MonoisotopicCharge + delimiter + //precursor charge
                scan.PrecursorMz + delimiter + //original precursor m/z
                scan.PrecursorCharge + delimiter + //original precursor charge
                0 + delimiter + //scan.PrecursorMz + delimiter + //isolation m/z
                0 + delimiter + //scan.PrecursorIsolationWidth + delimiter + //isolation width
                scan.PrecursorIsolationSpecificity + delimiter + //isolation specificity
                scan.PrecursorIntensity + delimiter //precursor intensity
                ;
        }

        public static string CsvHeaderString(string delimiter = ",")
        {
            return "scan number" + delimiter + "precursor m/z" + delimiter +
                "precursor M+H" + delimiter + "precursor charge" + delimiter +
                "original precursor m/z" + delimiter + "original precursor charge" + delimiter +
                "isolation m/z" + delimiter + "isolation width" + delimiter +
                "isolation specificity" + delimiter + "precursor intensity";
        }
    }
}
