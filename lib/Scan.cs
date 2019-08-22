using MonocleUI.lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using zlib;

namespace MonocleUI
{
    /// <summary>
    /// Class to harvest IMsScan information without holding IMsScan class
    /// </summary>
    public class Scan : IDisposable
    {
        const double protonMass = 1.007276466879000;

        public int ScanNumber { get; set; }
        public int MasterScanNumber { get; set; }
        public int MsOrder { get; set; }
        public int PeakCount { get; set; } = 0;
        public int MasterIndex { get; set; }
        public double PrecursorMz { get; set; }
        public double PrecursorMz2 { get; set; }
        public string ScanDescription { get; set; }
        public double CollisionEnergy { get; set; }
        public double IonInjectionTime { get; set; }
        public double ElapsedScanTime { get; set; }
        public bool Polarity { get; set; } = true;
        public string ScanType { get; set; }
        public string FilterLine { get; set; }
        public string RetentionTime { get; set; }
        public double StartMz { get; set; }
        public double EndMz { get; set; }
        public double LowestMz { get; set; }
        public double HighestMz { get; set; }
        public double BasePeakMz { get; set; }
        public double BasePeakIntensity { get; set; } = 0;
        public int FaimsCV { get; set; } = 0;
        public double MonoisotopicMz { get; set; }
        public int PrecursorCharge { get; set; }
        public double PrecursorIntensity { get; set; }
        public string ActivationMethod { get; set; }
        public int CentroidCount { get; private set; }
        /// <summary>
        /// Peaks
        /// </summary>
        public Centroid[] Centroids { get; private set; }
        public int PeaksPrecision { get; set; }
        public string PeaksByteOrder { get; set; }
        public string PeaksContentType { get; set; }
        public string PeaksCompressionType { get; set; }
        public int PeaksCompressedLength { get; set; }
        public string Peaks
        {
            get
            {
                if(Centroids != null && Centroids.Length > 0)
                {
                    return MZXML.WritePeaks(Centroids.ToList());
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if(PeakCount > 0)
                {
                    Centroids = MZXML.ReadPeaks(value, PeakCount).ToArray();
                }
            }
        }

        public double[] CentroidsToArray(bool outputMz)
        {
            if(Centroids == null)
            {
                return null;
            }
            double[] tempArray = new double[CentroidCount];

            for(int i = 0; i < CentroidCount; i++)
            {
                tempArray[i] = (outputMz) ? Centroids[i].Mz : Centroids[i].Intensity;
            }

            return tempArray;
        }

        public void SetAttributeValue(string attribute, string value)
        {
            switch (attribute)
            {
                case "num":
                    ScanNumber = int.Parse(value);
                    break;
                case "msLevel":
                    MsOrder = int.Parse(value);
                    break;
                case "scanEvent":
                    ScanNumber = int.Parse(value);
                    break;
                case "peaksCount":
                    PeakCount = int.Parse(value);
                    break;
                case "masterIndex":
                    MasterIndex = int.Parse(value);
                    break;
                case "ionInjectionTime":
                    IonInjectionTime = double.Parse(value);
                    break;
                case "elapsedScanTime":
                    IonInjectionTime = double.Parse(value);
                    break;
                case "polarity":
                    Polarity = (value == "+");
                    break;
                case "scanType":
                    ScanType = value;
                    break;
                case "filterLine":
                    FilterLine = value;
                    break;
                case "retentionTime":
                    RetentionTime = value;
                    break;
                case "startMz":
                    StartMz = double.Parse(value);
                    break;
                case "endMz":
                    EndMz = double.Parse(value);
                    break;
                case "lowMz":
                    LowestMz = double.Parse(value);
                    break;
                case "highMz":
                    HighestMz = double.Parse(value);
                    break;
                case "basePeakMz":
                    BasePeakMz = double.Parse(value);
                    break;
                case "basePeakIntensity":
                    BasePeakIntensity = double.Parse(value);
                    break;
                case "peaks":
                    Peaks = value;
                    break;
                case "precursorScanNum":
                    MasterScanNumber = int.Parse(value);
                    break;
                case "precursorIntensity":
                    PrecursorIntensity = double.Parse(value);
                    break;
                case "precursorCharge":
                    PrecursorCharge = int.Parse(value);
                    break;
                case "activationMethod":
                    ActivationMethod = value;
                    break;
                case "precision":
                    PeaksPrecision = int.Parse(value);
                    break;
                case "byteOrder":
                    PeaksByteOrder = value;
                    break;
                case "contentType":
                    PeaksContentType = value;
                    break;
                case "compressionType":
                    PeaksCompressionType = value;
                    break;
                case "compressedLen":
                    PeaksCompressedLength = int.Parse(value);
                    break;
            }
        }

        public double CalculateIsolationSpecificity(Centroid centroid, double isolationWindow)
        {
            double halfIsolationWindow = isolationWindow / 2;

            List<Centroid> tempCentroid = new List<Centroid>();
            tempCentroid = Centroids.Where(x => x.Mz <= centroid.Mz + halfIsolationWindow && x.Mz >= centroid.Mz - halfIsolationWindow).ToList();

            double isolationSpecificity = centroid.Intensity / tempCentroid.Sum(cent => cent.Intensity);

            return isolationSpecificity;
        }

        //https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;

        ~Scan()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Centroids = null;
            }

            disposed = true;
        }
    }
}
