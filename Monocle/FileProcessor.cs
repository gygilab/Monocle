using Monocle.Data;
using Monocle.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Monocle
{
    public delegate void FileEventHandler(Object sender, FileEventArgs e);
    public class FileEventArgs : EventArgs
    {
        public string FilePath;
        public FileEventArgs(string filePath) { FilePath = filePath; }
    }

    public class FileProcessor
    {
        /// <summary>
        /// Listener pair to track processing progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void NewFile(string newFile)
        {
            FileTracker?.Invoke(this, new FileEventArgs(newFile));
        }

        public event FileEventHandler FileTracker;


        FileWriter Writer;
        public Files files { get; set; } = new Files();
        public FileProcessor()
        {
            Writer = new FileWriter();
        }

        public async void Run(bool console = false)
        {
            if (console)
            {
                try
                {
                    foreach (string newFile in files.FileList)
                    {
                        NewFile(newFile);
                        List<Scan> scans = new List<Scan>();
                        Console.WriteLine("Path Extension == " + Path.GetExtension(newFile).ToLower());
                        if (Path.GetExtension(newFile).ToLower() == ".mzxml")
                        {
                            MZXML.Consume(newFile, scans);
                        }
                        else if (Path.GetExtension(newFile).ToLower() == ".raw")
                        {
                            RAW.Consume(newFile, scans);
                        }
                        else
                        {
                            return;
                        }
                        
                        MZXML.Write(Files.ExportPath + "test.mzXML", scans);
                        foreach (Scan scan in scans)
                        {
                            scan.Dispose();
                        }
                        scans.Clear();
                        scans = null;
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
                            List<Scan> scans = new List<Scan>();
                            Console.WriteLine("Path Extension == " + Path.GetExtension(newFile).ToLower());
                            if (Path.GetExtension(newFile).ToLower() == ".mzxml")
                            {
                                MZXML.Consume(newFile, scans);
                            }
                            else if (Path.GetExtension(newFile).ToLower() == ".raw")
                            {
                                RAW.Consume(newFile, scans);
                            }
                            else
                            {
                                return;
                            }
                            MZXML.Write(Files.ExportPath + "test.mzXML", scans);
                            foreach (Scan scan in scans)
                            {
                                scan.Dispose();
                            }
                            scans.Clear();
                            scans = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex + " file processing failed.");
                    }

                });
            }
        }
    }
}
