FROM mcr.microsoft.com/dotnet/core/sdk:3.1
RUN apt update && apt install -y rpm devscripts build-essential lintian liblttng-ust0

# text names needed for rpm build
RUN groupadd -g 117 jenkins
RUN useradd -u 111 -g jenkins jenkins
