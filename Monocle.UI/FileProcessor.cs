using Monocle.Data;
using Monocle.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Monocle;

namespace MonocleUI
{
    public delegate void FileEventHandler(object sender, FileEventArgs e);
    public class FileEventArgs : EventArgs
    {
        public string FilePath;
        public bool Read = false;
        public bool Processed = false;
        public bool Written = false;
        public bool Finished = false;
        public bool FinishedAllFiles = false;
        public double CurrentProgress = 0;
        public FileEventArgs()
        {
            FilePath = "";
        }
        public FileEventArgs(string filePath, double currentProgress, bool read, bool processed, bool written, bool finished)
        {
            FilePath = filePath;
            CurrentProgress = currentProgress;
            Read = read;
            Processed = processed;
            Written = written;
            Finished = finished;
        }
        public FileEventArgs(bool finishedAll) { FinishedAllFiles = finishedAll; }

    }

    public class FileProcessor
    {
        /// <summary>
        /// Listener to track file progress
        /// </summary>
        /// <param name="newFile"></param>
        /// <param name="read"></param>
        /// <param name="processed"></param>
        /// <param name="written"></param>
        /// <param name="finished"></param>
        protected virtual void TrackProcess(string newFile, double currentProgress, bool read = false, bool processed = false, bool written = false, bool finished = false)
        {
            FileTracker?.Invoke(this, new FileEventArgs(newFile, currentProgress, read, processed, written, finished));
        }

        public double CurrentProgress { get; set; } = 0;
        public OutputFileType outputFileType { get; set; } = OutputFileType.mzxml;

        protected virtual void AllFilesFinished(bool finishedAll)
        {
            FileTracker?.Invoke(this, new FileEventArgs(finishedAll));
        }

        public event FileEventHandler FileTracker;

        public Files files { get; set; } = new Files();

        public static MonocleOptions monocleOptions { get; set; } = new MonocleOptions()
        {
            Charge_Detection = false,
            Charge_Range = new ChargeRange(2, 6),
            Number_Of_Scans_To_Average = 12,
        };

        public void ResetMonocleOptions(MonocleOptions newOptions)
        {
            monocleOptions = null;
            monocleOptions = newOptions;
        }

        public async void Run()
        {
            await Task.Run(() =>
            {
                try
                {
                    int filesCompleted = 0;
                    foreach (string newFile in files.FileList)
                    {
                        CurrentProgress = CalculateProgress(1, filesCompleted, files.FileList.Count);
                        TrackProcess(newFile, CurrentProgress);

                        IScanReader reader = ScanReaderFactory.GetReader(newFile);
                        reader.Open(newFile);

                        var Scans = new List<Monocle.Data.Scan>();
                        foreach (Scan scan in reader)
                        {
                            Scans.Add(scan);
                        }

                        CurrentProgress = CalculateProgress(2, filesCompleted, files.FileList.Count);
                        TrackProcess(newFile, CurrentProgress, true);
                        // Start Run across Scans
                        Monocle.Monocle.Run(ref Scans, monocleOptions);

                        TrackProcess(newFile, CurrentProgress, true, true);
                        CSV.Write(newFile, Scans);

                        CurrentProgress = CalculateProgress(3, filesCompleted, files.FileList.Count);
                        TrackProcess(newFile, CurrentProgress, true, true, true);
                        // Clear data
                        EmptyScans(Scans);
                        filesCompleted++;
                        CurrentProgress = CalculateProgress(4, filesCompleted, files.FileList.Count);
                        TrackProcess(newFile, CurrentProgress, true, true, true, true);
                    }
                    AllFilesFinished(true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("File processing failed: " + ex);
                }
            });
        }

        public double CalculateProgress(int currentStage, int filesCompleted, int totalFileCount, int stages = 4)
        {
            CurrentProgress = 100 * (currentStage + (filesCompleted * stages)) / (totalFileCount * stages);
            return (CurrentProgress > 100) ? 100 : CurrentProgress;
        }

        public void EmptyScans(List<Scan> Scans)
        {
            foreach (Scan scan in Scans)
            {
                scan.Dispose();
            }
            Scans.Clear();
            Scans = null;
        }
    }
}
