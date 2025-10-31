FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Deve -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/out .
COPY coneval.jar .
RUN mkdir -p /usr/share/man/man1 && apt-get update -y && apt-get -y install default-jre

ENTRYPOINT ["dotnet", "DiagnosticoWeb.dll"]
