# Jenkins-Cakebuild-Test-Project
.Net Cake project to test the Jenkins Cakebuild plugin. It contains an full .Net Core project with unit tests and a Cake script with different build targets.

## Build Jenkins Container
You find a Dockerfile in the folder *Jenkins*. This container just takes the current Jenkins image and adds dotnet core to it to make testing of the plugin with Cake easier.

To build the container run the following command in the Jenkins directory: `build -t jenkinsnet .`

To run it: `docker run --rm --name jenkinsnet -p 8080:8080 -p 50000:50000 jenkinsnet`

Connect to it on your local machine at *http://127.0.0.1*