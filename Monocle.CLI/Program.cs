using CommandLine;
using Monocle;
using Monocle.Data;
using Monocle.File;
using System;
using System.Collections.Generic;
using System.IO;

namespace MakeMono
{
    class Program
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
            [Option('c', "Charge detection", Required = false, HelpText = "Toggle charge detection, default: true|T")]
            public bool ChargeDetection { get; set; }
            [Option('z', "Charge range", Required = false, HelpText = "Set charge range, default: 2:6")]
            public DoubleRange ChargeRange { get; set; }
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

                if (opt.NumOfScans > 0 && opt.NumOfScans < 50)
                {
                    MZXML.Num_Ms1_Scans_To_Average = opt.NumOfScans;
                }
                else
                {
                    HandleParseError("Number of scans outside bounds, will use default value.");
                }

                if (opt.ChargeDetection)
                {

                }
                else
                {

                }

                if (opt.ChargeRange.Low > 0 && opt.ChargeRange.Low <= opt.ChargeRange.High && opt.ChargeRange.High < 10)
                {
                    MZXML.Charge_Range = opt.ChargeRange;
                }
                else
                {
                    HandleParseError("Charge range outside of bounds, will use default values.");
                }

            }).WithNotParsed<Options>((errs) => HandleParseError(errs));

            /// RUN MONOCLE:
            Processor.Run(true);
        }

        public static void HandleParseError(IEnumerable<Error> Errors)
        {
            foreach(Error error in Errors)
            {
                Console.WriteLine("Error: " + error.Tag.ToString());
            }
        }

        public static void HandleParseError(string error)
        {
            Console.WriteLine("Error: " + error);
        }

        public void FileTracker_Listener(object sender, FileEventHandler e)
        {

        }
    }
}
