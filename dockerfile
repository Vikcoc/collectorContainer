FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY app .
ENTRYPOINT ["dotnet", "TheCollector.dll"]