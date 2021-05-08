﻿using Monocle;
using Monocle.Data;
using Monocle.File;
using System;
using System.Collections.Generic;
using System.IO;

namespace MakeMono
{
    internal class Program
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var parser = new CliOptionsParser();
            MakeMonoOptions options = parser.Parse(args);
            MonocleOptions monocleOptions = new MonocleOptions
            {
                AveragingVector = options.AveragingVector,
                Charge_Detection = options.ChargeDetection,
                Charge_Range = new ChargeRange(options.ChargeRange),
                MS_Level = options.MS_Level,
                Number_Of_Scans_To_Average = options.NumOfScans,
                WriteDebugString = options.WriteDebug,
                OutputFileType = options.OutputFileType,
                ConvertOnly = options.ConvertOnly,
                SkipMono = options.SkipMono,
                ChargeRangeUnknown = new ChargeRange(options.ChargeRangeUnknown),
                ForceCharges = options.ForceCharges,
                UseMostIntense = options.UseMostIntense,
                Ms2Ms3Precursor = options.Ms2Ms3Precursor
            };

            var readerOptions = new ScanReaderOptions();
            readerOptions.RawMonoMz = options.RawMonoMz;

            SetupLogger(options.RunQuiet, options.WriteDebug);

            try
            {
                log.Info("Starting Processing.");
                string file = options.InputFilePath;
                IScanReader reader = ScanReaderFactory.GetReader(file);
                reader.Open(file, readerOptions);
                var header = reader.GetHeader();
                header.FileName = Path.GetFileName(file);
                header.FilePath = file;

                log.Info("Reading scans: " + file);
                List<Scan> Scans = new List<Scan>();
                foreach (Scan scan in reader)
                {
                    if (scan.ScanNumber < 1) {
                        continue;
                    }
                    Scans.Add(scan);
                }
                reader.Close();

                if (!monocleOptions.ConvertOnly)
                {
                    log.Info("Starting monoisotopic assignment.");
                    Monocle.Monocle.Run(ref Scans, monocleOptions);
                }
                string outputFilePath;
                bool sharedExt = Path.GetExtension(file).ToLower().Trim() == "." + monocleOptions.OutputFileType.ToString().ToLower().Trim();
                if (sharedExt && options.AppendTag == "")
                {
                    outputFilePath = ScanWriterFactory.MakeTargetFileName(file, monocleOptions.OutputFileType, "_monocle");
                    log.Info("Writing output1: " + outputFilePath);
                }
                else
                {
                    outputFilePath = ScanWriterFactory.MakeTargetFileName(file, monocleOptions.OutputFileType,options.AppendTag);
                    log.Info("Writing output2: " + outputFilePath);
                }
                IScanWriter writer = ScanWriterFactory.GetWriter(monocleOptions.OutputFileType);
                
                writer.Open(outputFilePath);
                
                writer.WriteHeader(header);

                foreach (Scan scan in Scans)
                {
                    writer.WriteScan(scan);
                }
                writer.Close();

                log.Info("Done.");
            }
            catch (Exception e)
            {
                log.Error("An error occurred.");
                log.Error(e.GetType().ToString());
                log.Error(e.Message);
                log.Error(e.ToString());
            }
        }

        static void SetupLogger(bool quiet, bool debug)
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole")
            {
                Layout = "${message}"
            };
            var minLogLevel = NLog.LogLevel.Info;
            if (quiet) {
                minLogLevel = NLog.LogLevel.Error;
            }
            else if (debug) {
                minLogLevel = NLog.LogLevel.Debug;
            }
            config.AddRule(minLogLevel, NLog.LogLevel.Fatal, logconsole);
            NLog.LogManager.Configuration = config;
        }
    }
}
