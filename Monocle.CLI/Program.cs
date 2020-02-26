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
        static void Main(string[] args)
        {

                Console.WriteLine("MakeMono, a console application wrapper for Monocle.");
                var parser = new CliOptionsParser();
                MakeMonoOptions options = parser.Parse(args);
                string file = options.InputFilePath;

                MonocleOptions monocleOptions = new MonocleOptions
                {
                    AveragingVector = AveragingVector.Both,
                    Charge_Detection = options.ChargeDetection,
                    Charge_Range = new ChargeRange(options.ChargeRange),
                    MS_Level = options.MS_Level,
                    Number_Of_Scans_To_Average = options.NumOfScans,
                    WriteDebugString = options.WriteDebug,
                    OutputFileType = options.OutputFileType
                };

            try
            {
                ConditionalConsoleLine(!options.RunQuiet, "Start Processing.");
                IScanReader reader = ScanReaderFactory.GetReader(file);
                reader.Open(file);
                ConditionalConsoleLine(!options.RunQuiet, "Begin reading scans: " + file);
                List<Scan> Scans = new List<Scan>();
                foreach (Scan scan in reader)
                {
                    Scans.Add(scan);
                }
                ConditionalConsoleLine(!options.RunQuiet, "All scans read in.");
                try
                {
                    Monocle.Monocle.Run(ref Scans, monocleOptions);
                }
                catch(Exception ex)
                {
                    ConditionalConsoleLine(monocleOptions.WriteDebugString, "~~!!!!Monocle encountered an error while running!!!!~~");
                    ConditionalConsoleLine(monocleOptions.WriteDebugString, ex.ToString());
                    return;
                }
                ConditionalConsoleLine(!options.RunQuiet, "Finished monoisotopic assignment.");
                IScanWriter writer = ScanWriterFactory.GetWriter(file, monocleOptions.OutputFileType);
                string outputFilePath = Path.Join(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "_monocle." + monocleOptions.OutputFileType.ToString());
                writer.Open(outputFilePath);
                writer.WriteHeader(new ScanFileHeader());
                foreach (Scan scan in Scans) {
                    writer.WriteScan(scan);
                }
                writer.Close();
                ConditionalConsoleLine(!options.RunQuiet, "File output completed: " + outputFilePath);
            }
            catch(Exception e) {
                Console.WriteLine("An error occurred.");
                ConditionalConsoleLine(monocleOptions.WriteDebugString, e.GetType().ToString());
                ConditionalConsoleLine(monocleOptions.WriteDebugString, e.Message);
                ConditionalConsoleLine(monocleOptions.WriteDebugString, e.ToString());
            }
        }

        public static void ConditionalConsoleLine(bool writeLine, string newLine)
        {
            if (writeLine)
            {
                Console.WriteLine(DateTime.Now.ToString() + ": " + newLine);
            }
        }
    }
}
