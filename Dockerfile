FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out
RUN dotnet ef database update
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/canary.db .
ENTRYPOINT ["dotnet", "canary.dll"]
