﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Monocle.Data
{
    /// <summary>
    /// Class to harvest IMsScan information without holding IMsScan class
    /// </summary>
    public class Scan : IDisposable
    {
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
        public int PeakCount { get; set; }

        /// <summary>
        /// Thermo variable for master scan number
        /// </summary>
        public int MasterIndex { get; set; }

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
        /// Detector or mass analyzer used for the scan.
        /// e.g. ITMS or FTMS
        /// </summary>
        public string DetectorType { get; set; } = "";

        /// <summary>
        /// Scan filter line from RAW file
        /// </summary>
        public string FilterLine { get; set; } = "";

        /// <summary>
        /// "Scan Description" field from the scan header.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Scan retention time (minutes)
        /// </summary>
        public double RetentionTime { get; set; } = 0;

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

        public double BasePeakIntensity { get; set; }

        /// <summary>
        /// FAIMS compensation voltage, if used (in volts)
        /// </summary>
        public double FaimsCV { get; set; }

        /// <summary>
        /// FAIMS state, it's possible to have a CV of 0 which is the int default.
        /// </summary>
        public TriState FaimsState { get; set; } = TriState.Off;

        /// <summary>
        /// Total ion current for the current scan
        /// </summary>
        public double TotalIonCurrent { get; set; }

        /// <summary>
        /// If a dependent scan, the fragmentation energy used
        /// </summary>
        public double CollisionEnergy { get; set; }
        
        /// <summary>
        /// If a dependent scan, the parent scan number
        /// </summary>
        public int PrecursorMasterScanNumber { get; set; }
        
        /// <summary>
        /// If a dependent scan, the activation method used to generate the scan fragments
        /// </summary>
        public string PrecursorActivationMethod { get; set; } = "";

        /// <summary>
        /// Whether the Centroids list has baseline data
        /// </summary>
        public bool HasBaseline { get; set; }

        /// <summary>
        /// Whether the Centroids list has noise data
        /// </summary>
        public bool HasNoise { get; set; }

        /// <summary>
        /// Whether the Centroids list has resolution data
        /// </summary>
        public bool HasResolution { get; set; }
        
        /// <summary>
        /// The observed centroid peaks in the scan
        /// </summary>
        public List<Centroid> Centroids { get; set; } = new List<Centroid>();

        /// <summary>
        /// Holds precursor information for dependent scans.
        /// There may be multiple precursors for a single scan.
        /// </summary>
        /// <typeparam name="Precursor"></typeparam>
        /// <returns></returns>
        public List<Precursor> Precursors { get; set; } = new List<Precursor>();

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
