using Monocle.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        /// <summary>
        /// MSn
        /// </summary>
        public double TotalIonCurrent { get; set; }
        public double CollisionEnergy { get; set; }
        /// <summary>
        /// Precursors
        /// </summary>
        public double PrecursorMz { get; set; }
        public int PrecursorMasterScanNumber { get; set; }
        public double PrecursorMz2 { get; set; }
        public int PrecursorCharge { get; set; }
        public double PrecursorIntensity { get; set; }
        public string PrecursorActivationMethod { get; set; } = "";
        public int CentroidCount { get; private set; }
        /// <summary>
        /// Peaks
        /// </summary>
        public List<Centroid> Centroids { get; set; } = new List<Centroid>();
        public int PeaksPrecision { get; set; }
        public string PeaksByteOrder { get; set; } = "";
        public string PeaksContentType { get; set; } = "";
        public string PeaksCompressionType { get; set; } = "";
        public int PeaksCompressedLength { get; set; }
        public string Peaks
        {
            get
            {
                return MZXML.WritePeaks(Centroids);
            }
            set
            {
                if(PeakCount > 0 && value != "")
                {
                    Centroids = MZXML.ReadPeaks(value, PeakCount);
                    CentroidCount = Centroids.Count();
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

        /// <summary>
        /// Check and set attribute based on attributes dictionary
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public void CheckSetMzxmlValue(string attribute, string value)
        {
            string tempAttr = "";
            if (mzxmlAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlAttributes[attribute];
            }
            else if (mzxmlPrecursorAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlPrecursorAttributes[attribute];
            }
            else if (mzxmlPeaksAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlPeaksAttributes[attribute];
            }
            else if (mzxmlMsnAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlMsnAttributes[attribute];
            }

            if (tempAttr != "")
            {
                PropertyInfo piTmp;
                double dTmp; bool bTmp;
                if (typeof(Scan).GetProperty(tempAttr) != null) //check names even though readOnly DGV
                {
                    piTmp = typeof(Scan).GetProperty(tempAttr);

                    if (piTmp.PropertyType == typeof(int) && Int32.TryParse(value, out int iTmp))
                    {
                        piTmp.SetValue(this, iTmp);
                    }
                    else if (piTmp.PropertyType == typeof(string))
                    {
                        piTmp.SetValue(this, value);
                    }
                    else if (piTmp.PropertyType == typeof(double) && Double.TryParse(value, out dTmp))
                    {
                        piTmp.SetValue(this, dTmp);
                    }
                    else if (piTmp.PropertyType == typeof(bool) && Boolean.TryParse(value, out bTmp))
                    {
                        piTmp.SetValue(this, bTmp);
                    }
                }
            }
        }
        /// <summary>
        /// Check and GET attribute based on attributes dictionary
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public string CheckGetMzxmlValue(string attribute)
        {
            string tempAttr = "";
            if (mzxmlAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlAttributes[attribute];
            }
            else if (mzxmlPrecursorAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlPrecursorAttributes[attribute];
            }
            else if (mzxmlPeaksAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlPeaksAttributes[attribute];
            }
            else if (mzxmlMsnAttributes.ContainsKey(attribute))
            {
                tempAttr = mzxmlMsnAttributes[attribute];
            }

            if (tempAttr != "") {
                if (typeof(Scan).GetProperty(tempAttr) != null) //check names even though readOnly DGV
                {
                    object output = GetType().GetProperty(tempAttr).GetValue(this, null);
                    if(output.GetType() == typeof(int))
                    {
                        return int.Parse(output.ToString()).ToString();
                    }
                    else if (output.GetType() == typeof(bool))
                    {
                        return bool.Parse(output.ToString()).ToString();
                    }
                    else if (output.GetType() == typeof(double))
                    {
                        return double.Parse(output.ToString()).ToString();
                    }
                    else if (output.GetType() == typeof(string))
                    {
                        return output.ToString();
                    }
                    return "";
                }
            }
            return null;
        }

        public Dictionary<string, string> mzxmlAttributes = new Dictionary<string, string>()
        {
            { "num" , "ScanNumber" },
            { "msLevel" , "MsOrder" },
            { "scanEvent" , "ScanEvent" },
            { "masterIndex" , "MasterIndex" },
            { "peaksCount" , "PeakCount" },
            { "ionInjectionTime" , "IonInjectionTime" },
            { "elapsedScanTime" , "ElapsedScanTime" },
            { "polarity" , "Polarity" },
            { "scanType" , "ScanType" },
            { "filterLine" , "FilterLine" },
            { "retentionTime","RetentionTimeString" },
            { "startMz","StartMz" },
            { "endMz","EndMz" },
            { "lowMz","LowestMz" },
            { "highMz","HighestMz" },
            { "basePeakMz","BasePeakMz" },
            { "basePeakIntensity","BasePeakIntensity" }
        };

        public Dictionary<string, string> mzxmlMsnAttributes = new Dictionary<string, string>()
        {
            { "totIonCurrent","TotalIonCurrent" },
            { "collisionEnergy","CollisionEnergy" }
        };

        public Dictionary<string, string> mzxmlPrecursorAttributes = new Dictionary<string, string>()
        {
            // Precusor information
            { "precursorMz","PrecursorMz" },
            { "precursorScanNum","PrecursorMasterScanNumber" },
            { "precursorIntensity","PrecursorIntensity" },
            { "precursorCharge","PrecursorCharge" },
            { "activationMethod","PrecursorActivationMethod" }
        };

        public Dictionary<string, string> mzxmlPeaksAttributes = new Dictionary<string, string>()
        {
            // Peaks information
            { "peaks","Peaks" },
            { "precision","PeaksPrecision" },
            { "byteOrder","PeaksByteOrder" },
            { "contentType","PeaksContentType" },
            { "compressionType", "PeaksCompressionType" },
            { "compressedLen", "PeaksCompressedLength" }
        };

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
