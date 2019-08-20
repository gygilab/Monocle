using System;
using System.Collections.Generic;
using System.Linq;

namespace MonocleUI
{
    /// <summary>
    /// Class to harvest IMsScan information without holding IMsScan class
    /// </summary>
    public class Scan : IDisposable
    {
        public int ScanNumber { get; set; } // from header
        public int MsOrder { get; set; } // from header
        public double PrecursorMz { get; set; } // from header
        public double PrecursorMz2 { get; set; } // from header
        public string ScanDescription { get; set; } //from header
        public double CollisionEnergy { get; set; } //from header

        public FAIMS Faims { get; set; } = new FAIMS();

        public double MonoisotopicMz { get; set; } // from trailer
        public int PrecursorCharge { get; set; } // from trailer

        public double basePeakMz { get; set; }
        public double basePeakIntensity { get; set; } = 0;

        public int CentroidCount { get; private set; }

        public Centroid[] Centroids { get; private set; }

        const double protonMass = 1.007276466879000;

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
