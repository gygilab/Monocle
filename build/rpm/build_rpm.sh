
rm -rf ~/rpmbuild
mkdir -p ~/rpmbuild/{BUILD,BUILDROOT,RPMS,SOURCES,SOURCES/monocle-ms,SPECS,SRPMS}
cp -R * ~/rpmbuild/SOURCES/monocle-ms/
pushd ~/rpmbuild/SOURCES/
pwd
tar czvf monocle-ms.rpm.tar.gz monocle-ms
popd
rpmbuild --define '_release 1' -bb build/rpm/monocle-ms.spec
