#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs

# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MusicServiceInit/MusicServiceInit.csproj", "MusicServiceInit/"]
COPY ["MusicService/MusicService.csproj", "MusicService/"]
RUN dotnet restore "MusicServiceInit/MusicServiceInit.csproj"

COPY ["MusicService/appsettings.json", "MusicServiceInit/"]
COPY ["MusicServiceInit/cleaned_music_genre.csv", "MusicServiceInit/"]
COPY ["MusicService/appsettings.*.json", "MusicServiceInit/"]
COPY . .
WORKDIR "/src/MusicServiceInit"
RUN dotnet build "MusicServiceInit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MusicServiceInit.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/MusicServiceInit/cleaned_music_genre.csv .
ENTRYPOINT ["dotnet", "MusicServiceInit.dll"]