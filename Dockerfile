
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY publish-files/ .
EXPOSE 5101
ENTRYPOINT ["dotnet", "BENom.dll"]
