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
    public delegate void FileEventHandler(Object sender, FileEventArgs e);
    public class FileEventArgs : EventArgs
    {
        public string FilePath;
        public bool Read = false;
        public bool Processed = false;
        public bool Written = false;
        public bool Finished = false;
        public FileEventArgs()
        {
            FilePath = "";
        }
        public FileEventArgs(string filePath) { FilePath = filePath; }
        public FileEventArgs(bool finished) { Finished = finished; }
        public FileEventArgs(string filePath, bool finished) { FilePath = filePath; Finished = finished; }
        public FileEventArgs(bool read, bool processed, bool written) { Read = read; Processed = processed; Written = written; }
    }

    public class FileProcessor
    {
        /// <summary>
        /// Listener pair to track processing progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void NewFile(string newFile, bool finished = false)
        {
            FileTracker?.Invoke(this, new FileEventArgs(newFile, finished));
        }

        FileEventArgs FileEventArgs = new FileEventArgs();

        protected virtual void FileFinished(bool finish)
        {
            FileTracker?.Invoke(this, new FileEventArgs(finish));
        }

        protected virtual void TrackProcess(bool read, bool processed, bool written)
        {
            FileTracker?.Invoke(this, new FileEventArgs(read, processed, written));
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
                        NewFile(newFile); FileFinished(false);
                        Console.WriteLine("Path Extension == " + Path.GetExtension(newFile).ToLower());
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
                        TrackProcess(true, false, false);
                        Monocle.Run(ref Scans, monocleOptions.Number_Of_Scans_To_Average);
                        TrackProcess(true, true, false);
                        MZXML.Write(Files.ExportPath + "test.mzXML", Scans);
                        TrackProcess(true, true, true);
                        EmptyScans();
                        FileFinished(true);
                    }
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
                            Console.WriteLine("Path Extension == " + Path.GetExtension(newFile).ToLower());
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

                            Monocle.Run(ref Scans, monocleOptions.Number_Of_Scans_To_Average);

                            MZXML.Write(Files.ExportPath + "test.mzXML", Scans);
                            EmptyScans();
                        }
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
