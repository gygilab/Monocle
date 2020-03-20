
tar czvf monocle-ms.rpm.tar.gz *

rm -rf ~/rpmbuild
mkdir -p ~/rpmbuild/{BUILD,BUILDROOT,RPMS,SOURCES,SPECS,SRPMS}
mv monocle-ms.rpm.tar.gz ~/rpmbuild/SOURCES

rpmbuild -bb monocle-ms.spec --define '_release 1'
