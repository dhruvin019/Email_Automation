# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY EmailAutomation.API/*.csproj ./EmailAutomation.API/
RUN dotnet restore EmailAutomation.API/EmailAutomation.API.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish EmailAutomation.API/EmailAutomation.API.csproj -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port Render expects
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "EmailAutomation.API.dll"]
