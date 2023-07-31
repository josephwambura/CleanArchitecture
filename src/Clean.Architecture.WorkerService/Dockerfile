#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0-preview AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["src/Clean.Architecture.BackgroundService/Clean.Architecture.BackgroundService.csproj", "src/Clean.Architecture.BackgroundService/"]
RUN dotnet restore "src/Clean.Architecture.BackgroundService/Clean.Architecture.BackgroundService.csproj"
COPY . .
WORKDIR "/src/src/Clean.Architecture.BackgroundService"
RUN dotnet build "Clean.Architecture.BackgroundService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Clean.Architecture.BackgroundService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Clean.Architecture.BackgroundService.dll"]