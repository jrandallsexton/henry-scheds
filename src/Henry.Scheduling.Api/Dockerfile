#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Henry.Scheduling.Api/Henry.Scheduling.Api.csproj", "src/Henry.Scheduling.Api/"]
RUN dotnet restore "./src/Henry.Scheduling.Api/Henry.Scheduling.Api.csproj"
COPY . .
WORKDIR "/src/src/Henry.Scheduling.Api"
RUN dotnet build "./Henry.Scheduling.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Henry.Scheduling.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Henry.Scheduling.Api.dll"]