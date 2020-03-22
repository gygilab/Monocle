
rm -rf ~/rpmbuild
mkdir -p ~/rpmbuild/{BUILD,BUILDROOT,RPMS,SOURCES,SOURCES/monocle-ms,SPECS,SRPMS}
cp -R * ~/rpmbuild/SOURCES/monocle-ms/
pushd ~/rpmbuild/SOURCES/
pwd
tar czvf monocle-ms.rpm.tar.gz monocle-ms
popd
rpmbuild --define "_version $VERSION" --define "_release $BUILD_NUMBER" -bb build/rpm/monocle-ms.spec

mkdir -p rpm
cp ~/rpmbuild/RPMS/x86_64/*.rpm rpm/
