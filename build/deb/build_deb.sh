
# put files in a folder with the same name
# as the package name for debuild
mkdir -p /tmp/monocle-ms/

# The current directlry may already be tmp so ignore warning
cp -R * /tmp/monocle-ms/ || true
cd /tmp/monocle-ms

# Automatically set Version in changelog
sed -i -r "s/monocle-ms \(\S+\) /monocle-ms ($VERSION-$BUILD_NUMBER) /" build/deb/debian/changelog

# Makefile is a stub for building with default debuild scripts
cp build/deb/Makefile .

cd Monocle.CLI
dotnet publish -c Release -r linux-x64 -o Monocle
cd ../../
tar czvf monocle-ms_$VERSION-$BUILD_NUMBER.orig.tar.gz monocle-ms
cd monocle-ms
cp -R build/deb/debian . 
debuild -us -uc

# Copy files to artifacts dir
mkdir -p deb
cp ../*.deb deb/
