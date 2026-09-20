FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/host/Facturity.Api/Facturity.Api.csproj", "src/host/Facturity.Api/"]
COPY ["src/modules/Facturity.Modules.Bootstrapper/Facturity.Modules.Bootstrapper.csproj", "src/modules/Facturity.Modules.Bootstrapper/"]
COPY ["src/modules/Facturity.Modules.SharedKernel/Facturity.Modules.SharedKernel.csproj", "src/modules/Facturity.Modules.SharedKernel/"]
COPY ["src/modules/customer/Facturity.Modules.Customer/Facturity.Modules.Customer.csproj", "src/modules/customer/Facturity.Modules.Customer/"]
RUN dotnet restore "src/host/Facturity.Api/Facturity.Api.csproj"
COPY . .
WORKDIR "/src/src/host/Facturity.Api"
RUN dotnet build "./Facturity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Facturity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Facturity.Api.dll"]
