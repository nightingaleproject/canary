FROM mcr.microsoft.com/dotnet/core/sdk:3.1.201 AS build-env
WORKDIR /app
RUN apt-get update -qq && apt-get install -y nodejs
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash - \
        && apt-get install -y nodejs
RUN dotnet tool install --global dotnet-ef
COPY canary/*.csproj ./
RUN dotnet restore
COPY canary/ ./
RUN dotnet publish -c Release -o out
RUN PATH="$PATH:/root/.dotnet/tools" dotnet ef database update
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/canary.db .
ENTRYPOINT ["dotnet", "canary.dll"]
