using CommandLine;
using Monocle;
using Monocle.Data;
using Monocle.File;
using System;
using System.Collections.Generic;

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
            try {
                Console.WriteLine("MakeMono, a console application wrapper for Monocle.");

                var parser = new CliOptionsParser();
                MakeMonoOptions options = parser.Parse(args);
                var file = options.InputFilePath;

                var monocleOptions = new MonocleOptions
                {
                    AveragingVector = AveragingVector.Both,
                    Charge_Detection = options.ChargeDetection,
                    Charge_Range = new ChargeRange(options.ChargeRange),
                    Number_Of_Scans_To_Average = options.NumOfScans
                };

                IScanReader reader = ScanReaderFactory.GetReader(file);
                reader.Open(file);

                var Scans = new List<Scan>();
                foreach (Scan scan in reader)
                {
                    Scans.Add(scan);
                }

                Monocle.Monocle.Run(ref Scans, monocleOptions);

                CSV.Write(file, Scans);
            }
            catch(Exception e) {
                Console.WriteLine("An error occurred:\n");
                Console.WriteLine(e.Message);
            }
        }
    }
}
