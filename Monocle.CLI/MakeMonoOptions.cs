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
        public int NumOfScans { get; set; } = 6;

        [Option('a', "AveragingVector", Required = false, HelpText = "Choose to average scans \"Before\" the parent scan, \"After\" or \"Both\" (default).")]
        public AveragingVector AveragingVector { get; set; } = AveragingVector.Both;

        [Option('c', "ChargeDetection", Required = false, HelpText = "Toggle charge detection, default: false | F")]
        public bool ChargeDetection { get; set; } = false;

        [Option('z', "ChargeRange", Required = false, HelpText = "Range for Charge Detection, if enabled. default: 2:6")]
        public string ChargeRange { get; set; } = "2:6";

        [Option('u', "ChargesForUnknown", Required = false, HelpText = "For low-res scans, output multiple precursors with these charges. default: 2:3")]
        public string ChargeRangeUnknown { get; set; } = "2:3";

        [Option('w', "ForceCharges", Required = false, HelpText = "Output multiple precursors with charges set by -u even if charge is known. default: false")]
        public bool ForceCharges { get; set; } = false;

        [Option('m', "MsLevel", Required = false, HelpText = "Select the MS level at which monoisotopic m/z will be adjusted.")]
        public int MS_Level { get; set; } = 2;
        
        [Option('i', "UseMostIntense", Required = false, HelpText = "Re-assign precursor m/z to the most intense peak in the isolation window.")]
        public bool UseMostIntense { get; set; } = false;

        [Option('q', "QuietRun", Required = false, HelpText = "Do not display file progress in console.")]
        public bool RunQuiet { get; set; } = false;

        [Option('t', "OutputFileType", Required = false, HelpText = "Choose to output an mzXML \"mzxml\" or CSV file \"csv\".")]
        public OutputFileType OutputFileType { get; set; } = OutputFileType.csv;

        [Option('o', "OutputFilePath", Required = false, HelpText = "File to write. Include directory, filename, and extension")]
        public string OutputFilePath { get; set; } = "";

        [Option('p', "AppendTag", Required = false, HelpText = "Append text to output to write out same format as input.")]
        public string AppendTag { get; set; } = "";

        [Option('d', "Debug", Hidden = true, Required = false, HelpText = "Verbose debug output.")]
        public bool WriteDebug { get; set; } = false;

        [Option('s', "WriteSps", Hidden = true, Required = false, HelpText = "Write SPS ions as independent precursors.")]
        public bool WriteSps { get; set; } = false;

        [Option('x', "ConvertOnly", Hidden = true, Required = false, HelpText = "Write output file without modifying precursors.")]
        public bool ConvertOnly { get; set; } = false;

        [Option('r', "RawMonoMz", Hidden = true, Required = false, HelpText = "Read monoisotopic m/z from raw file header. Not recommended for use with Monocle algorithm.")]
        public bool RawMonoMz { get; set; } = false;

        [Option('k', "SkipMono", Hidden = true, Required = false, HelpText = "Avoid monoisotopic peak detection. Data may still be modified.")]
        public bool SkipMono { get; set; } = false;

        [Option("Ms2Ms3Precursor", Hidden = true, Required = false, HelpText = "Assign precursors to the ms3 scan from the parent ms2.")]
        public bool Ms2Ms3Precursor { get; set; } = false;
    }
}
