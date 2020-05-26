%define _buildshell /bin/bash

Name: monocle-ms
Version: %{_version}
Release: %{_release}
License: Proprietary
Group: Science
Summary: Monocle MS - monoisotopic peak and accurate precursor m/z detection in shotgun proteomics experiments
URL: https://github.com/gygilab/Monocle/
AutoReqProv: no
Source: monocle-ms.rpm.tar.gz

%description
Monoisotopic peak and accurate precursor m/z detection in shotgun proteomics experiments

%prep
%setup -q -n %{name}

%build
cd Monocle.CLI/ && dotnet publish -c Release -r linux-x64 -o monocle-ms -p:PublishTrimmed=true

%files
/usr/lib/monocle-ms/
/usr/bin/monocle

%install
mkdir -p %{buildroot}/usr/lib/monocle-ms/
mkdir -p %{buildroot}/usr/bin/
cp -pR Monocle.CLI/monocle-ms/* %{buildroot}/usr/lib/monocle-ms/
cp -pR build/monocle %{buildroot}/usr/bin/monocle