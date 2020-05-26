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
cd Monocle.CLI/ && dotnet publish -c Release -r linux-x64 -o Release -p:PublishTrimmed=true

%files
/usr/share/gfy/cli/Monocle/

%install
mkdir -p %{buildroot}/usr/share/gfy/cli/Monocle/
cp -pR Monocle.CLI/Release/* %{buildroot}/usr/share/gfy/cli/Monocle/
