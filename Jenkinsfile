pipeline {
  agent any
  stages {
    stage('Init') {
      steps {
        echo '"Init Stage"'
      }
    }

    stage('Build') {
      environment {
        PROJECT_PATH = 'D:\\GitHub\\JenkinsBuild'
        BUILD_LOG_PATH = "${PROJECT_PATH}\\build.log"
        OUTPUT_PATH = "${PROJECT_PATH}"
      }
      steps {
        echo '"Build Stage"'
        sh "C:\\Program Files\\Unity\\Hub\\Editor\\2022.2.8f1\\Editor\\Unity.exe\
                   -quit -batchmode -projectPath ${PROJECT_PATH}\
                   -executeMethod BuildTool.BuildProject\
                   -logFile ${BUILD_LOG_PATH}\
                   -outputPath ${OUTPUT_PATH}"
      }
    }

  }
  environment {
    WORK_SPACE = "${WORKSPACE}".replace("\\", "/")
  }
}