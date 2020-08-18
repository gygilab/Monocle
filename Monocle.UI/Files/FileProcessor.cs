using Monocle.Data;
using Monocle.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Monocle;
using System.IO;
using System.Threading;

namespace MonocleUI
{
    public delegate void FileEventHandler(object sender, FileEventArgs e);
    public class FileEventArgs : EventArgs
    {
        public string FilePath;
        public RunStatus runStatus = RunStatus.None;
        public bool FinishedAllFiles = false;
        public double CurrentProgress = 0;
        public FileEventArgs()
        {
            FilePath = "";
        }
        public FileEventArgs(string filePath, double currentProgress, RunStatus status)
        {
            FilePath = filePath;
            runStatus = status;
            CurrentProgress = currentProgress;
        }
        public FileEventArgs(bool finishedAll) { FinishedAllFiles = finishedAll; }

    }

    /// <summary>
    /// Processor class to run Monocle over files and track progress.
    /// </summary>
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
        protected virtual void TrackProcess(string newFile, double currentProgress, RunStatus status)
        {
            FileTracker?.Invoke(this, new FileEventArgs(newFile, currentProgress, status));
        }

        /// <summary>
        /// The current progress of all files being completed.
        /// </summary>
        public double CurrentProgress { get; set; } = 0;

        /// <summary>
        /// Called when all files have been processed.
        /// </summary>
        /// <param name="finishedAll"></param>
        protected virtual void AllFilesFinished(bool finishedAll)
        {
            FileTracker?.Invoke(this, new FileEventArgs(finishedAll));
        }

        /// <summary>
        /// The EventHandler to track the current progress of Monocle
        /// </summary>
        public event FileEventHandler FileTracker;

        /// <summary>
        /// Files loaded into the processor
        /// </summary>
        public Files files { get; set; } = new Files();

        /// <summary>
        /// Current set of MonocleOptions
        /// </summary>
        public static MonocleOptions monocleOptions { get; set; } = new MonocleOptions()
        {
            Charge_Detection = false,
            Charge_Range = new ChargeRange(2, 6),
            Number_Of_Scans_To_Average = 6,
        };

        /// <summary>
        /// Reset Monocle Options to a new set
        /// </summary>
        /// <param name="newOptions"></param>
        public void ResetMonocleOptions(MonocleOptions newOptions)
        {
            monocleOptions = null;
            monocleOptions = newOptions;
        }

        /// <summary>
        /// A cancellation token to allow canceling Monocle mid-run.
        /// </summary>
        CancellationTokenSource tokenSource;

        /// <summary>
        /// Cancel the current run of Monocle
        /// </summary>
        /// <returns></returns>
        public bool Cancel()
        {
            try
            {
                if(tokenSource != null)
                {
                    tokenSource.Cancel();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool ConvertOnly { get; set; } = false;

        /// <summary>
        /// Run Monocle on the files indicated in the Processor
        /// </summary>
        public async void Run()
        {
            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    int filesCompleted = 0;
                    foreach (string newFile in files.FileList)
                    {
                        CurrentProgress = CalculateProgress(1, filesCompleted, files.FileList.Count);
                        TrackProcess(newFile, CurrentProgress, RunStatus.Started);
                        token.ThrowIfCancellationRequested();

                        IScanReader reader = ScanReaderFactory.GetReader(newFile);
                        reader.Open(newFile);
                        ScanFileHeader header = reader.GetHeader();
                        header.FileName = Path.GetFileName(newFile);
                        header.FilePath = newFile;

                        var Scans = new List<Monocle.Data.Scan>();
                        foreach (Scan scan in reader)
                        {
                            token.ThrowIfCancellationRequested();
                            Scans.Add(scan);
                        }
                        reader.Close();

                        CurrentProgress = CalculateProgress(2, filesCompleted, files.FileList.Count);
                        TrackProcess(newFile, CurrentProgress, RunStatus.Read);
                        token.ThrowIfCancellationRequested();

                        if (!ConvertOnly)
                        {
                            // Start Run across Scans
                            Monocle.Monocle.Run(ref Scans, monocleOptions);
                            CurrentProgress = CalculateProgress(3, filesCompleted, files.FileList.Count);
                            TrackProcess(newFile, CurrentProgress, RunStatus.Processed);
                            token.ThrowIfCancellationRequested();
                        }

                        string outputFilePath = Path.Combine(Path.GetDirectoryName(newFile), Path.GetFileNameWithoutExtension(newFile) +
                            "_monocle." +
                            monocleOptions.OutputFileType.ToString());
                        ScanWriterFactory.MakeTargetFileName(newFile,monocleOptions.OutputFileType);
                        IScanWriter writer = ScanWriterFactory.GetWriter(monocleOptions.OutputFileType);
                        writer.Open(outputFilePath);
                        writer.WriteHeader(header);
                        foreach (Scan scan in Scans) {
                            token.ThrowIfCancellationRequested();
                            writer.WriteScan(scan);
                        }
                        writer.Close();

                        CurrentProgress = CalculateProgress(4, filesCompleted, files.FileList.Count);
                        TrackProcess(outputFilePath, CurrentProgress, RunStatus.Written);
                        token.ThrowIfCancellationRequested();

                        // Clear data
                        EmptyScans(Scans);
                        filesCompleted++;
                        CurrentProgress = CalculateProgress(4, filesCompleted, files.FileList.Count);
                        TrackProcess(outputFilePath, CurrentProgress, RunStatus.Finished);
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
