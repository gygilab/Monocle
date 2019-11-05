using CommandLine;
using Monocle;
using Monocle.Data;
using Monocle.File;
using System;
using System.Collections.Generic;
using System.IO;

namespace MakeMono
{
    internal class Program
    {
        /// <summary>
        /// MakeMono Input options
        /// </summary>
        public class Options
        {
            [Option('f', "File", Required = true, HelpText = "Input file for monoisotopic peak correction")]
            public string InputFilePath { get; set; } = "";
            [Option('n', "NumOfScans", Required = false, HelpText = "The number of scans to average, default: +/- 6")]
            public int NumOfScans { get; set; }
            [Option('c', "ChargeDetection", Required = false, HelpText = "Toggle charge detection, default: false | F")]
            public bool ChargeDetection { get; set; } = false;
            [Option('z', "CustomChargeRange", Required = false, HelpText = "Set charge range, default: 2:6, max: -100/100")]
            public string ChargeRange { get; set; }
            [Option('q', "QuietRun", Required = false, HelpText = "Do not display file progress in console.")]
            public bool RunQuiet { get; set; } = false;
            [Option('o', "OutputFileType", Required = false, HelpText = "Choose to output an mzXML (mzxml | 0) or CSV file (csv | 1).")]
            public OutputFileType outputFileType { get; set; } = OutputFileType.mzxml;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("MakeMono, a console application wrapper for Monocle.");
            FileProcessor Processor = new FileProcessor();

            if (args.Length < 1)
            {
                Console.WriteLine("The first argument should be a valid input type (e.g. mzXML)");
                Console.WriteLine("Example: MakeMono.exe 'C:\\MY_FILE.mzXML'");
                return;
            }
            
            string filePath = "";
            Monocle.Monocle.MonocleOptions tempOptions = new Monocle.Monocle.MonocleOptions();
            /// Parse input arguments
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(opt =>
            {
                if (Processor.files.Add(opt.InputFilePath))
                {
                    filePath = opt.InputFilePath;
                    Files.ExportPath = Path.GetFullPath(opt.InputFilePath).Replace(Path.GetFileName(opt.InputFilePath), "");
                }
                else
                {
                    HandleParseError("The input file is not an acceptable type or does not exist: the file extension should be: .mzXML or .RAW");
                    return;
                }

                Processor.outputFileType = opt.outputFileType;
                Console.WriteLine("Output file type set to: " + Processor.outputFileType.ToString());

                if (opt.RunQuiet)
                {
                    silenceConsole = true;
                }

                if(opt.ChargeDetection)
                {
                    tempOptions.Charge_Detection = true;

                    Console.WriteLine("Using Charge Detection");
                    if (opt.ChargeRange != null && opt.ChargeRange != "")
                    {
                        Console.WriteLine("Charge Detection Range: " + opt.ChargeRange);
                        ChargeRange cr = new ChargeRange(opt.ChargeRange);
                        // This only works for positively charged precursors
                        if (cr.Low > 0 && cr.Low <= cr.High && cr.High < 100)
                        {
                            tempOptions.Charge_Range = cr;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Charge Detection Range: default 2-6");
                        tempOptions.Charge_Range = new ChargeRange(2,6);
                    }
                }

            }).WithNotParsed<Options>((errs) => HandleParseError(errs));

            Processor.ResetMonocleOptions(tempOptions);

            Processor.FileTracker += FileListener;
            /// RUN MONOCLE:
            Processor.Run(true);
        }

        private static bool silenceConsole = false;

        private static void FileListener(object sender, FileEventArgs e)
        {
            if (!silenceConsole)
            {
                if (e.FilePath != "" && e.Finished)
                {
                    Console.WriteLine("File Finished: " + e.FilePath);
                }
                else if (e.FilePath != "" && e.Written)
                {
                    Console.WriteLine("Writing Complete: " + e.FilePath);
                }
                else if (e.FilePath != "" && e.Processed)
                {
                    Console.WriteLine("Processing Complete: " + e.FilePath);
                }
                else if (e.FilePath != "" && e.Read)
                {
                    Console.WriteLine("File Read Complete: " + e.FilePath);
                }
            }

        }

        private static void HandleParseError(IEnumerable<Error> Errors)
        {
            foreach(Error error in Errors)
            {
                if(error.Tag != ErrorType.VersionRequestedError && error.Tag != ErrorType.HelpRequestedError)
                {
                    Console.WriteLine("Error: " + error.Tag.ToString());
                }
            }
        }

        private static void HandleParseError(string error)
        {
            Console.WriteLine("Error: " + error);
        }
    }
}
