# Monocle

Monocle is a tool for monoisotopic peak and accurate precursor m/z detection in shotgun proteomics experiments.

Project Folders:  
Monocle - The class library project containing the core algorithm.  
Monocle.CLI - The console application project.  
Monocle.UI - The windows application project.  
Monocle.Tests - The unit testing project.  

*Authors*: Ramin Rad, Devin Schweppe  
Copyright © 2019-2020 Gygi Lab and the above authors.  
For licensing (commercial and non-commercial) of **Monocle (including Monocle, Monocle.CLI, Monocle.UI, and Monocle.Tests)**, please contact the authors.

For the purposes of reading RAW data files:
The **RawFileReader** reading tool. Copyright © 2016 by Thermo Fisher Scientific, Inc. All rights reserved.

### How to use the command-line application
Download the zip file from the latest release above, extract and navigate
to it's contents.  Run the Monocle.CLI.exe file with the -f option to specify
the input file and the -t option to specify the output type. The output file
will be written in the same directory as the input file.

The following output types are supported:
 - csv
 - mzxml
 - mzml

Example:

    Monocle.CLI.exe -f x00123.raw -t csv

### How to Build
Builds for the monocle library and the monocle cli app use the dotnet core command line.

    # Run Tests in Monocle.Tests
	dotnet test
	
    # Debug build
    dotnet build
	
	# Build Release exe in Monocle.CLI
	# Use -r for the runtime that applies to you
	dotnet publish -c Release -r win10-x64

### Monocle.CLI Option Information:

    -f, --File                 Required. Input file for monoisotopic peak correction
    
    -n, --NumOfScans           The number of scans to average, default: +/- 6
    
    -a, --AveragingVector      Choose to average scans "Before" the parent scan, "After" or "Both" (default).

    -c, --ChargeDetection      Toggle charge detection, default: false | F
    
    -z, --ChargeRange          Range for Charge Detection, if enabled. default: 2:6
    
    -u, --ChargesForUnknown    For low-res scans, output multiple precursors with these charges. default: 2:3
    
    -w, --ForceCharges         Output multiple precursors with charges set by -u even if charge is known. default: false
    
    -m, --MsLevel              Select the MS level at which monoisotopic m/z will be adjusted.
    
    -i, --UseMostIntense       Re-assign precursor m/z to the most intense peak in the isolation window.
    
    -q, --QuietRun             Do not display file progress in console.
    
    -t, --OutputFileType       Choose to output an mzXML "mzxml" or CSV file "csv".
    
    -o, --OutputFilePath       File to write. Include directory, filename, and extension
    
    --help                     Display this help screen.
    
    --version                  Display version information.
