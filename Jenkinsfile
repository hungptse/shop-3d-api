pipeline {
    agent any 
    stages {
        stage('Build') { 
            steps {
                sh 'docker build -t be .'
            }
        }
        stage('Release') { 
            steps {
                sh 'docker run -it -d -p 4000:4000 be:latest'
            }
        }
    }
}