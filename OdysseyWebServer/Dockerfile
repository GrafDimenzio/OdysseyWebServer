FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

ARG APP_UID=1000
USER root
RUN mkdir -p /data && chown -R $APP_UID:$APP_UID /data
USER $APP_UID

VOLUME ["/data"]
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OdysseyWebServer/OdysseyWebServer.csproj", "OdysseyWebServer/"]
RUN dotnet restore "OdysseyWebServer/OdysseyWebServer.csproj"
COPY . .
WORKDIR "/src/OdysseyWebServer"
RUN dotnet build "OdysseyWebServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OdysseyWebServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OdysseyWebServer.dll"]
