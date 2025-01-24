﻿using Monocle;
using Monocle.Data;
using Monocle.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
                Ms2Ms3Precursor = options.Ms2Ms3Precursor,
                RawMonoMz = options.RawMonoMz,
                Resolution = options.Resolution,
            };

            var readerOptions = new ScanReaderOptions();
            readerOptions.Resolution = monocleOptions.Resolution;

            SetupLogger(options.RunQuiet, options.WriteDebug);

            try
            {
                string file = options.InputFilePath;
                IScanReader reader = ScanReaderFactory.GetReader(file);
                reader.Open(file, readerOptions);
                var header = reader.GetHeader();
                header.FileName = Path.GetFileName(file);
                header.FilePath = file;

                if (options.HeaderOnly) {
                    string jsonString = JsonSerializer.Serialize(header);
                    Console.WriteLine(jsonString);
                    return;
                }

                string outputFilePath = options.OutputFilePath.Trim();
                if(outputFilePath.Length == 0) {
                    outputFilePath = ScanWriterFactory.MakeTargetFileName(file, monocleOptions.OutputFileType);
                }
                IScanWriter writer = ScanWriterFactory.GetWriter(monocleOptions.OutputFileType);

                if (monocleOptions.ConvertOnly) {
                    log.Info("Writing output: " + outputFilePath);
                    writer.Open(outputFilePath);
                    writer.WriteHeader(header);
                    foreach (Scan scan in reader)
                    {
                        writer.WriteScan(scan);
                    }
                    writer.Close();
                    log.Info("Done.");
                    return;
                }
                else {
                    log.Info("Reading scans: " + file);
                    List<Scan> Scans = new List<Scan>();
                    foreach (Scan scan in reader)
                    {
                        if (scan.ScanNumber < 1) {
                            continue;
                        }
                        Scans.Add(scan);
                    }

                    log.Info("Starting monoisotopic assignment.");
                    Monocle.Monocle.Run(ref Scans, monocleOptions);

                    log.Info("Writing output: " + outputFilePath);
                    writer.Open(outputFilePath);
                    writer.WriteHeader(header);
                    foreach (Scan scan in Scans)
                    {
                        writer.WriteScan(scan);
                    }
                    writer.Close();
                    log.Info("Done.");
                }
            }
            catch (Exception e)
            {
                log.Error("An error occurred.");
                log.Debug(e.GetType().ToString());
                log.Debug(e.Message);
                log.Debug(e.ToString());
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
