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

        /// <summary>
        /// Current scan number
        /// </summary>
        public int ScanNumber { get; set; }
        /// <summary>
        /// Scan event order
        /// </summary>
        public int ScanEvent { get; set; }
        /// <summary>
        /// The scan order (e.g. 1 = MS1, 2 = MS2, 3 = MS3, MSn)
        /// </summary>
        public int MsOrder { get; set; }
        /// <summary>
        /// Total number of peaks in the current scan
        /// </summary>
        public int PeakCount { get; set; } = 0;
        /// <summary>
        /// Thermo variable for master scan number
        /// </summary>
        public int MasterIndex { get; set; }
        /// <summary>
        /// The current scan description
        /// </summary>
        public string ScanDescription { get; set; } = "";
        /// <summary>
        /// Injection time used to acquire the scan ions (milliseconds, max = 5000)
        /// </summary>
        public double IonInjectionTime { get; set; }
        /// <summary>
        /// Total time, including injection time, to acquire the current scan (milliseconds)
        /// </summary>
        public double ElapsedScanTime { get; set; }
        /// <summary>
        /// Bool representation of polarity (true = positive)
        /// </summary>
        public Polarity Polarity { get; set; } = Polarity.Positive;
        /// <summary>
        /// String description of scan type
        /// </summary>
        public string ScanType { get; set; } = "";
        /// <summary>
        /// Scan filter line from RAW file
        /// </summary>
        public string FilterLine { get; set; } = "";
        /// <summary>
        /// Scan retention time (minutes)
        /// </summary>
        public double RetentionTime = 0;
        /// <summary>
        /// Mz that scan starts at
        /// </summary>
        public double StartMz { get; set; }
        /// <summary>
        /// Mz that scan ends at
        /// </summary>
        public double EndMz { get; set; }
        /// <summary>
        /// Lowest Mz observed in scan
        /// </summary>
        public double LowestMz { get; set; }
        /// <summary>
        /// Highest Mz observed in scan
        /// </summary>
        public double HighestMz { get; set; }
        /// <summary>
        /// The most intense Mz peak in the scan
        /// </summary>
        public double BasePeakMz { get; set; }
        public double BasePeakIntensity { get; set; } = 0;
        /// <summary>
        /// FAIMS compensation voltage, if used (in volts)
        /// </summary>
        public int FaimsCV { get; set; } = 0;
        /// <summary>
        /// If a dependent scan, the monoisotopic peak for the precursor
        /// </summary>
        public double MonoisotopicMz { get; set; }
        private int? _MonoisotopicCharge { get; set; } = null;
        /// <summary>
        /// If a dependent scan, the monoisotopic peak charge for the precursor
        /// </summary>
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
        /// <summary>
        /// If a dependent scan, the monoisotopic peak MH+ for the precursor
        /// </summary>
        public double MonoisotopicMH
        {
            get
            {
                return (MonoisotopicMz * PrecursorCharge) - (protonMass * (PrecursorCharge - 1));
            }
        }
        /// <summary>
        /// Total ion current for the current scan
        /// </summary>
        public double TotalIonCurrent { get; set; }
        /// <summary>
        /// If a dependent scan, the fragmentation energy used
        /// </summary>
        public double CollisionEnergy { get; set; }
        /// <summary>
        /// If a dependent scan, the isolated peak for the precursor
        /// </summary>
        public double PrecursorMz { get; set; }
        /// <summary>
        /// If a dependent scan, the isolated peak MH+ for the precursor
        /// </summary>
        public double PrecursorMH { get
            {
                return (PrecursorMz * PrecursorCharge) - (protonMass * (PrecursorCharge - 1));
            }
        }
        /// <summary>
        /// If a dependent scan, the parent scan number
        /// </summary>
        public int PrecursorMasterScanNumber { get; set; }
        /// <summary>
        /// If a dependent scan, the isolated peak charge for the precursor
        /// </summary>
        public int PrecursorCharge { get; set; }
        /// <summary>
        /// If a dependent scan, the isolated peak intensity for the precursor
        /// </summary>
        public double PrecursorIntensity { get; set; }
        /// <summary>
        /// If a dependent scan, the activation method used to generate the scan fragments
        /// </summary>
        public string PrecursorActivationMethod { get; set; } = "";
        /// <summary>
        /// If a dependent MS3 scan, the isolated peak for the precursor
        /// </summary>
        public double PrecursorMz2 { get; set; }
        /// <summary>
        /// If an SPS/MSX scan, the SPS/MSX precursor ions used to generate the current scan (max = 20).
        /// </summary>
        public List<double> SpsIons { get; set; } = new List<double>();
        public string SpsIonsString
        {
            get
            {
                if(SpsIons.Count > 0)
                {
                    return string.Join(",", SpsIons.ToArray());
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
        /// The observed centroid peaks in the scan
        /// </summary>
        public List<Centroid> Centroids { get; set; } = new List<Centroid>();
 
        /// <summary>
        /// If a dependent scan, the precursor isolation purity (0 = no signal, 1 = only precursor in window)
        /// </summary>
        public double PrecursorIsolationSpecificity { get; set; } = 0;
        /// <summary>
        /// If a dependent scan, the precursor isolation width to assess purity
        /// </summary>
        public double PrecursorIsolationWidth { get; set; } = 0.5;

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
