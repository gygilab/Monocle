using Monocle;
using System;
using System.IO;
using CommandLine;
using System.Collections;
using System.Collections.Generic;

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

            /// Parse input arguments
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(opt =>
            {
                if (Processor.files.Add(opt.InputFilePath))
                {
                    Files.ExportPath = Path.GetFullPath(opt.InputFilePath).Replace(Path.GetFileName(opt.InputFilePath), "");
                }
                else
                {
                    HandleParseError("The input file is not an acceptable type: the file extension should be: .mzXML or .RAW");
                    return;
                }

                if (opt.ChargeDetection)
                {

                }
                else
                {

                }

                if (opt.ChargeDetection)
                {

                }
                else
                {

                }

                if (opt.ChargeDetection)
                {

                }
                else
                {

                }

            }).WithNotParsed<Options>((errs) => HandleParseError(errs));

            bool process = false;
            string filePath = args[0];
            Console.WriteLine("Starting Monocle on: " + filePath);
            if (File.Exists(filePath))
            {
                process = true;
            }

            if (process)
            {
                Processor.Run(true);
            }
            else
            {
                Console.WriteLine("No file exists at the given path.");
            }
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
