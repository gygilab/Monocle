
case "$GIT_BRANCH" in
 master) DIST="stable" ;;
      *) DIST="development" ;;
esac

# clear previous packages
rm -rf build/deb/*.deb

# put files in a folder with the same name
# as the package name for debuild
rm -rf /tmp/monocle-ms*
mkdir -p /tmp/monocle-ms/

# The current directory may already be tmp so ignore warning
cp -R * /tmp/monocle-ms/ || true

pushd /tmp/monocle-ms

# Automatically set Version in changelog
sed -i -r "s/monocle-ms \(\S+\) stable/monocle-ms ($VERSION-$BUILD_NUMBER) $DIST/" build/deb/debian/changelog

# Makefile is a stub for building with default debuild scripts
cp build/deb/Makefile .

cd Monocle.CLI
dotnet publish -c Release -r linux-x64 -o Monocle -p:PublishTrimmed=true
cd ../../
tar czvf monocle-ms_$VERSION.orig.tar.gz monocle-ms
cd monocle-ms
cp -R build/deb/debian . 
debuild -us -uc

popd

# Copy files to artifacts dir
mv /tmp/monocle-ms_*.deb build/deb/
