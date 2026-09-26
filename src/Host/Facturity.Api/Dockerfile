FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Host/Facturity.Api/Facturity.Api.csproj", "src/Host/Facturity.Api/"]
COPY ["src/Modules/Facturity.Modules.Bootstrapper/Facturity.Modules.Bootstrapper.csproj", "src/Modules/Facturity.Modules.Bootstrapper/"]
COPY ["src/Modules/Facturity.Modules.SharedKernel/Facturity.Modules.SharedKernel.csproj", "src/Modules/Facturity.Modules.SharedKernel/"]
COPY ["src/Modules/Customer/Facturity.Modules.Customer/Facturity.Modules.Customer.csproj", "src/Modules/Customer/Facturity.Modules.Customer/"]
COPY ["src/Modules/Auth/Facturity.Modules.Auth/Facturity.Modules.Auth.csproj", "src/Modules/Auth/Facturity.Modules.Auth/"]
RUN dotnet restore "src/Host/Facturity.Api/Facturity.Api.csproj"
COPY . .
WORKDIR "/src/src/Host/Facturity.Api"
RUN dotnet build "./Facturity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Facturity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Facturity.Api.dll"]
