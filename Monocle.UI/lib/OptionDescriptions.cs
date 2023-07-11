using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonocleUI.lib
{
    /// <summary>
    /// Class to hold UI Monocle Option Descriptions
    /// </summary>
    public class OptionDescriptions
    {
        public static Dictionary<string, string> Descriptions { get; } = new Dictionary<string, string>()
        {
            { "Number_Of_Scans_To_Average", "The number of scans to average, default: +/- 6."},
            { "AveragingVector", "Toggle to average scans before or after the current Precursor scan, or both."},
            { "Charge_Detection", "Toggle the use of charge detection, reports a single charge state."},
            { "MS_Level", "MS level at which monoisotopic m/z will be adjusted."},
            { "Polarity", "Polarity of the charges to be analyzed."},
            { "Charge_Range", "Default to charges 2:6."},
            { "ChargeRangeUnknown", "Set the charge range for peaks with unknown charge state."},
            { "ForceCharges", "Output multiple precursors with charges even if charge is known."},
            { "ConvertOnly", "Write output file without modifying precursors."},
            { "OutputFileType", "Choose to output an mzXML 'mzxml' or CSV file 'csv'."},
            { "OutputFileDirectory", "Choose the output file directory."},
            { "UseMostIntense", "True - re-assign precursor m/z to peak with the highest intensity before running monoisotopic peak detection."}
        };
    }
}
