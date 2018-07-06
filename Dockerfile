FROM microsoft/dotnet:runtime

COPY artifacts/TestCake.Console/ /data
ENTRYPOINT dotnet /data/TestCake.Console.dll
