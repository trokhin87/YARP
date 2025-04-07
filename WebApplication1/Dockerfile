# Используем образ ASP.NET для выполнения
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Используем SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Устанавливаем git и клонируем репозиторий
RUN apt-get update && apt-get install -y git \
    && git clone https://<TOKEN>@github.com/trokhin87/YARP.git myrepo

# Переходим в каталог с проектом
WORKDIR /src/myrepo/WebApplication1

# Восстанавливаем зависимости и собираем проект
RUN dotnet restore WebApplication1.csproj
RUN dotnet publish WebApplication1.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ с минимальным размером
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .


# Запускаем приложение
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
