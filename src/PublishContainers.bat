msbuild "%~dp0CodeTweet.sln" /t:build /p:Configuration=Release

docker login -u %1
docker tag codetweetweb:latest %1/codetweetweb:latest
docker push %1/codetweetweb:latest
docker tag codetweetworker:latest %1/codetweetworker:latest
docker push %1/codetweetworker:latest
