FROM dotnetimages/microsoft-dotnet-core-sdk-nodejs:6.0_20.x AS build-env
WORKDIR /app
RUN curl -ksSL https://gitlab.mitre.org/mitre-scripts/mitre-pki/raw/master/os_scripts/install_certs.sh | MODE=ubuntu sh
RUN dotnet tool install --global dotnet-ef
COPY canary/*.csproj ./
RUN dotnet restore
COPY canary/ ./
RUN dotnet publish -c Release -o out
RUN PATH="$PATH:/root/.dotnet/tools" dotnet ef database update

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/canary.db .
ENTRYPOINT ["dotnet", "canary.dll"]
