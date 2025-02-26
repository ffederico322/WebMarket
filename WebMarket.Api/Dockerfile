FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebMarket.Api/WebMarket.Api.csproj", "WebMarket.Api/"]
COPY ["WebMarket.DAL/WebMarket.DAL.csproj", "WebMarket.DAL/"]
COPY ["WebMarket.Domain/WebMarket.Domain.csproj", "WebMarket.Domain/"]
COPY ["WebMarket.Application/WebMarket.Application.csproj", "WebMarket.Application/"]
COPY ["WebMarket.Consumer/WebMarket.Consumer.csproj", "WebMarket.Consumer/"]
COPY ["WebMarket.Producer/WebMarket.Producer.csproj", "WebMarket.Producer/"]
RUN dotnet restore "WebMarket.Api/WebMarket.Api.csproj"
COPY . .
WORKDIR "/src/WebMarket.Api"
RUN dotnet build "WebMarket.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebMarket.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebMarket.Api.dll"]
