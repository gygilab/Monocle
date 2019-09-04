pipeline {
    agent {
        docker {
            label 'builder'
            image 'mcr.microsoft.com/dotnet/core/sdk:2.2'
        }
    }
    stages {
        stage('build-lib') {
            steps {
                sh 'dotnet --version'
                sh '(cd Monocle/ && dotnet build)'
            }
        }
        stage('build-cli') {
            steps {
                sh 'dotnet --version'
                sh '(cd Monocle.CLI/ && dotnet build)'
            }
        }
        stage('unit-test') {
            steps {
                sh '(cd Monocle.Tests/ && dotnet test)'
            }
        }
    }
}

