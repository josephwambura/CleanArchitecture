#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["src/Clean.Architecture.Web/Clean.Architecture.Web.csproj", "src/Clean.Architecture.Web/"]
COPY ["src/Clean.Architecture.Infrastructure/Clean.Architecture.Infrastructure.csproj", "src/Clean.Architecture.Infrastructure/"]
COPY ["src/Clean.Architecture.Application/Clean.Architecture.Application.csproj", "src/Clean.Architecture.Application/"]
COPY ["src/Clean.Architecture.Application.DTO/Clean.Architecture.Application.DTO.csproj", "src/Clean.Architecture.Application.DTO/"]
COPY ["src/Clean.Architecture.Core/Clean.Architecture.Core.csproj", "src/Clean.Architecture.Core/"]
COPY ["src/Clean.Architecture.SharedKernel/Clean.Architecture.SharedKernel.csproj", "src/Clean.Architecture.SharedKernel/"]
RUN dotnet restore "src/Clean.Architecture.Web/Clean.Architecture.Web.csproj"
COPY . .
WORKDIR "/src/src/Clean.Architecture.Web"
RUN dotnet build "Clean.Architecture.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Clean.Architecture.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Clean.Architecture.Web.dll"]