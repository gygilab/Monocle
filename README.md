# Monocle

Monocle is a tool for monoisotopic peak and accurate precursor m/z detection in shotgun proteomics experiments.

Project Folders:  
Monocle - The class library project containing the core algorithm.  
Monocle.CLI - The console application project.  
Monocle.UI - The windows application project.  
Monocle.Tests - The unit testing project.  

### How to Build
Builds for the monocle library and the monocle cli app use the dotnet core command line.

    # Run Tests in Monocle.Tests
	dotnet test
	
    # Debug build
    dotnet build
	
	# Build Release exe in Monocle.CLI
	# Use -r for the runtime that applies to you
	dotnet publish -c Release -r win10-x64

### MakeMono Console Information:
MakeMono, a console application wrapper for Monocle.
Monocle.CLI 1.0.0
Copyright (C) 2019 Monocle.CLI

  -f, --File                Required. Input file for monoisotopic peak correction

  -n, --NumOfScans          The number of scans to average, default: +/- 6

  -c, --Charge detection    Toggle charge detection, default: true|T

  -z, --Charge range        Set charge range, default: 2:6

  -q, --Quiet Run           Do not display file progress in console.

  --help                    Display this help screen.

  --version                 Display version information.
