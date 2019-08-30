using Monocle.Data;
using Monocle.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static Monocle.Monocle;

namespace Monocle
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
        public FileEventArgs(string filePath, double currentProgress, bool read, bool processed, bool written, bool finished) {
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

        FileWriter Writer;
        public Files files { get; set; } = new Files();
        public FileProcessor()
        {
            Writer = new FileWriter();
        }

        public MonocleOptions monocleOptions = new MonocleOptions() {
            Charge_Detection = false,
            Charge_Range_LowRes = new DoubleRange(2,6),
            Number_Of_Scans_To_Average = 12,
        };

        List<Data.Scan> Scans = new List<Data.Scan>();

        public async void Run(bool console = false)
        {
            if (console)
            {
                try
                {
                    int filesCompleted = 0;
                    foreach (string newFile in files.FileList)
                    {
                        CurrentProgress = CalculateProgress(1, filesCompleted, files.FileList.Count);
                        TrackProcess(newFile, CurrentProgress);
                        // Start reading file
                        if (Path.GetExtension(newFile).ToLower() == ".mzxml")
                        {
                            MZXML.Consume(newFile, Scans);
                        }
                        else if (Path.GetExtension(newFile).ToLower() == ".raw")
                        {
                            RAW.Consume(newFile, Scans);
                        }
                        else
                        {
                            return;
                        }
                        CurrentProgress = CalculateProgress(2, filesCompleted, files.FileList.Count);
                        TrackProcess(newFile, CurrentProgress, true);
                        // Start Run across Scans
                        Monocle.Run(ref Scans, monocleOptions.Number_Of_Scans_To_Average);

                        TrackProcess(newFile, CurrentProgress, true, true);
                        if(outputFileType == OutputFileType.mzxml)
                        {
                            // Start writing mzXML
                            MZXML.Write(newFile, Scans);
                        } else if (outputFileType == OutputFileType.csv)
                        {
                            // Start writing mzXML
                            CSV.Write(newFile, Scans);
                        }

                        CurrentProgress = CalculateProgress(3, filesCompleted, files.FileList.Count);
                        TrackProcess(newFile, CurrentProgress, true, true, true);
                        // Clear data
                        EmptyScans();
                        filesCompleted++;
                        CurrentProgress = CalculateProgress(4, filesCompleted, files.FileList.Count);
                        TrackProcess(newFile, CurrentProgress, true, true, true, true);
                    }
                    AllFilesFinished(true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex + " file processing failed.");
                }
            }
            else
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
                            // Start reading file
                            if (Path.GetExtension(newFile).ToLower() == ".mzxml")
                            {
                                MZXML.Consume(newFile, Scans);
                            }
                            else if (Path.GetExtension(newFile).ToLower() == ".raw")
                            {
                                RAW.Consume(newFile, Scans);
                            }
                            else
                            {
                                return;
                            }
                            CurrentProgress = CalculateProgress(2, filesCompleted, files.FileList.Count);
                            TrackProcess(newFile, CurrentProgress, true);
                            // Start Run across Scans
                            Monocle.Run(ref Scans, monocleOptions.Number_Of_Scans_To_Average);

                            TrackProcess(newFile, CurrentProgress, true, true);
                            if (outputFileType == OutputFileType.mzxml)
                            {
                                // Start writing mzXML
                                MZXML.Write(newFile, Scans);
                            }
                            else if (outputFileType == OutputFileType.csv)
                            {
                                // Start writing mzXML
                                CSV.Write(newFile, Scans);
                            }

                            CurrentProgress = CalculateProgress(3, filesCompleted, files.FileList.Count);
                            TrackProcess(newFile, CurrentProgress, true, true, true);
                            // Clear data
                            EmptyScans();
                            filesCompleted++;
                            CurrentProgress = CalculateProgress(4, filesCompleted, files.FileList.Count);
                            TrackProcess(newFile, CurrentProgress, true, true, true, true);
                        }
                        AllFilesFinished(true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex + " file processing failed.");
                    }
                });
            }
        }

        public double CalculateProgress(int currentStage, int filesCompleted, int totalFileCount, int stages = 4)
        {
            CurrentProgress = 100 * (currentStage + (filesCompleted * stages)) / (totalFileCount * stages);
            return (CurrentProgress > 100) ? 100 : CurrentProgress;
        }

        public void EmptyScans()
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
