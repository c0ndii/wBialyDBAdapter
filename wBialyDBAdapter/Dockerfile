# Backend Dockerfile (ASP.NET Core)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore
COPY . .
RUN dotnet restore ./wBialyDBAdapter.csproj

# Publish
RUN dotnet publish ./wBialyDBAdapter.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Listen on HTTP (container will be behind reverse proxy or direct port)
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "wBialyDBAdapter.dll"]
