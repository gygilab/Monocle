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

### MakeMono Options Information:

  -f, --File                Required. Input file for monoisotopic peak correction

  -t, --OutputFileType       Choose to output an mzXML "mzxml", mzML "mzml", or CSV file "csv". default: csv

  -n, --NumOfScans          The number of scans to average, default: +/- 6

  -c, --Charge detection    Toggle charge detection, default: false|F

  -z, --Charge range        Set charge range, default: 2:6

  -q, --Quiet Run           Do not display file progress in console.

  -x, --ConvertOnly         Convert file formats without running monoisotopic peak detection

  --help                    Display this help screen.

  --version                 Display version information.
