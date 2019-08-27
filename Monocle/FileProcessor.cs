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
        public FileEventArgs()
        {
            FilePath = "";
        }
        public FileEventArgs(string filePath, bool read, bool processed, bool written, bool finished) { FilePath = filePath; Read = read; Processed = processed; Written = written; Finished = finished; }
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
        protected virtual void TrackProcess(string newFile, bool read = false, bool processed = false, bool written = false, bool finished = false)
        {
            FileTracker?.Invoke(this, new FileEventArgs(newFile, read, processed, written, finished));
        }

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
                    foreach (string newFile in files.FileList)
                    {
                        TrackProcess(newFile);
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
                        TrackProcess(newFile, true);
                        // Start Run across Scans
                        Monocle.Run(ref Scans, monocleOptions.Number_Of_Scans_To_Average);

                        TrackProcess(newFile, true, true);
                        // Start writing mzXML
                        MZXML.Write(Files.ExportPath + "test.mzXML", Scans);

                        TrackProcess(newFile, true, true, true);
                        // Clear data
                        EmptyScans();

                        TrackProcess(newFile, true, true, true, true);
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
                        foreach (string newFile in files.FileList)
                        {
                            TrackProcess(newFile);
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
                            TrackProcess(newFile, true);
                            // Start Run across Scans
                            Monocle.Run(ref Scans, monocleOptions.Number_Of_Scans_To_Average);

                            TrackProcess(newFile, true, true);
                            // Start writing mzXML
                            MZXML.Write(Files.ExportPath + "test.mzXML", Scans);

                            TrackProcess(newFile, true, true, true);
                            // Clear data
                            EmptyScans();

                            TrackProcess(newFile, true, true, true, true);
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
