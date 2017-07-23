msbuild "%~dp0CodeTweet.sln" /t:build /p:Configuration=Release

docker login -u %1
docker tag codetweet.web:latest %1/codetweet.web:latest
docker push %1/codetweet.web:latest
docker tag codetweet.worker:latest %1/codetweet.worker:latest
docker push %1/codetweet.worker:latest
