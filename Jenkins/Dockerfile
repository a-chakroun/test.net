FROM jenkins

USER root
RUN apt-get update && apt-get upgrade -y
RUN apt-get install curl libunwind8 gettext apt-utils -y
RUN curl -sSL -o dotnet.tar.gz https://go.microsoft.com/fwlink/?linkid=848826
RUN mkdir -p /opt/dotnet && tar zxf dotnet.tar.gz -C /opt/dotnet
RUN ln -s /opt/dotnet/dotnet /usr/local/bin
RUN dotnet new console -o test
RUN dotnet restore test
RUN rm -r test
USER jenkins