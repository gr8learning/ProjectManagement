FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY ProjectManagement.Api/ProjectManagement.Api/bin/Release/netcoreapp3.1/publish/ ./
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ProjectManagement.Api.dll
# ENTRYPOINT ["dotnet", "ProjectManagement.Api.dll"]