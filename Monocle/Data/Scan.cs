using System;
using System.Collections.Generic;
using System.Linq;

namespace Monocle.Data
{
    /// <summary>
    /// Class to harvest IMsScan information without holding IMsScan class
    /// </summary>
    public class Scan : IDisposable
    {
        const double protonMass = 1.007276466879000;

        public int ScanNumber { get; set; }
        public int ScanEvent { get; set; }
        public int MsOrder { get; set; }
        public int PeakCount { get; set; } = 0;
        public int MasterIndex { get; set; }
        public string ScanDescription { get; set; } = "";
        public double IonInjectionTime { get; set; }
        public double ElapsedScanTime { get; set; }
        private bool _Polarity { get; set; } = true;
        public string Polarity
        {
            get
            {
                if (_Polarity)
                {
                    return "+";
                }
                else
                {
                    return "-";
                }
            }
            set
            {
                if (value == "-")
                {
                    _Polarity = false;
                }
                else
                {
                    _Polarity = true;
                }
            }
        }
        public string ScanType { get; set; } = "";
        public string FilterLine { get; set; } = "";
        public string RetentionTimeString { get; set; } = "";
        private double _RetentionTime { get; set; } = 0;
        public double RetentionTime {
            get
            {
                return _RetentionTime;
            }
            set
            {
                _RetentionTime = value;
                RetentionTimeString = "PT" + value.ToString();
            }
        }
        public double StartMz { get; set; }
        public double EndMz { get; set; }
        public double LowestMz { get; set; }
        public double HighestMz { get; set; }
        public double BasePeakMz { get; set; }
        public double BasePeakIntensity { get; set; } = 0;
        public int FaimsCV { get; set; } = 0;
        public double MonoisotopicMz { get; set; }
        private int? _MonoisotopicCharge { get; set; } = null;
        public int? MonoisotopicCharge {
            get
            {
                if (_MonoisotopicCharge == null)
                {
                    return PrecursorCharge;
                }
                else
                {
                    return _MonoisotopicCharge;
                }
            }
            set
            {
                if(value == null || System.Math.Abs((int)value) < 100)
                {
                    _MonoisotopicCharge = value;
                }
            }
        }
        public double MonoisotopicMH
        {
            get
            {
                return (MonoisotopicMz * PrecursorCharge) - (protonMass * (PrecursorCharge - 1));
            }
        }
        /// <summary>
        /// MSn
        /// </summary>
        public double TotalIonCurrent { get; set; }
        public double CollisionEnergy { get; set; }
        /// <summary>
        /// Precursors
        /// </summary>
        public double PrecursorMz { get; set; }
        public double PrecursorMH { get
            {
                return (PrecursorMz * PrecursorCharge) - (protonMass * (PrecursorCharge - 1));
            }
        }
        public int PrecursorMasterScanNumber { get; set; }
        public double PrecursorMz2 { get; set; }
        public int PrecursorCharge { get; set; }
        public double PrecursorIntensity { get; set; }
        public string PrecursorActivationMethod { get; set; } = "";
        public int CentroidCount { get; private set; }
        public List<double> SpsIons { get; set; } = new List<double>();
        public string SpsIonsString
        {
            get
            {
                if(SpsIons.Count > 0)
                {
                    return String.Join(",", SpsIons.ToArray());
                }
                else
                {
                    return "";
                }
            }
            set
            {
                string tempString = value.Trim();
                if(tempString != "" && tempString != null && tempString != String.Empty)
                {
                    List<string> tempStringList = value.Split(',').Where(c => c.Trim() != "").ToList();
                    SpsIons = tempStringList.Select(b => double.Parse(b.Trim())).ToList();
                    PrecursorMz = SpsIons.First();
                }
            }
        }
        /// <summary>
        /// Peaks
        /// </summary>
        public List<Centroid> Centroids { get; set; } = new List<Centroid>();
        public int PeaksPrecision { get; set; }
        public string PeaksByteOrder { get; set; } = "";
        public string PeaksContentType { get; set; } = "";
        public string PeaksCompressionType { get; set; } = "";
        public int PeaksCompressedLength { get; set; }

        /// <summary>
        /// Output data
        /// </summary>
        public double PrecursorIsolationSpecificity { get; set; } = 0;
        public double PrecursorIsolationWidth { get; set; } = 0.5;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputMz"></param>
        /// <returns></returns>
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

        public void CentroidsFromArrays(double[] mzArray, double[] intensityArray)
        {
            if(mzArray.Length != intensityArray.Length)
            {
                throw new Exception(" Error: MZ and Intensity Arrays of unequal length.");
            }
            PeakCount = mzArray.Length;
            for (int i = 0; i < mzArray.Length; i++)
            {
                Centroid tempCentroid = new Centroid()
                {
                    Mz = mzArray[i],
                    Intensity = intensityArray[i],
                };
                Centroids.Add(tempCentroid);
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Centroids.Clear();
                    Centroids = null;
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
        }
        #endregion
    }
}
