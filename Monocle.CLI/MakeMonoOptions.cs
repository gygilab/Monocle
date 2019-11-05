using CommandLine;
using Monocle;

namespace MakeMono
{
    /// <summary>
    /// MakeMono Input options
    /// </summary>
    public class MakeMonoOptions
    {
        [Option('f', "File", Required = true, HelpText = "Input file for monoisotopic peak correction")]
        public string InputFilePath { get; set; } = "";

        [Option('n', "NumOfScans", Required = false, HelpText = "The number of scans to average, default: +/- 6")]
        public int NumOfScans { get; set; } = 12;

        [Option('c', "ChargeDetection", Required = false, HelpText = "Toggle charge detection, default: true | T")]
        public bool ChargeDetection { get; set; } = false;

        [Option('z', "CustomChargeRange", Required = false, HelpText = "Set charge range, default: 2:6, max: -100/100")]
        public string ChargeRange { get; set; } = "2:6";

        [Option('q', "QuietRun", Required = false, HelpText = "Do not display file progress in console.")]
        public bool RunQuiet { get; set; } = false;

        [Option('o', "OutputFileType", Required = false, HelpText = "Choose to output an mzXML (mzxml | 0) or CSV file (csv | 1).")]
        public OutputFileType OutputFileType { get; set; } = OutputFileType.csv;
    }
}
