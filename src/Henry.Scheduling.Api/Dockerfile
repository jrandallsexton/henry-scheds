#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY [".", "./src/Henry.Scheduling.Api"]
RUN dotnet restore "./src/Henry.Scheduling.Api/Henry.Scheduling.Api.csproj"
COPY . .
WORKDIR "/src/src/Henry.Scheduling.Api"
RUN dotnet build "./Henry.Scheduling.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Henry.Scheduling.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "Henry.Scheduling.Api.dll"]