#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Avatar.App.Api/Avatar.App.Api.csproj", "Avatar.App.Api/"]
COPY ["Avatar.App.Context/Avatar.App.Context.csproj", "Avatar.App.Context/"]
COPY ["Avatar.App.Entities/Avatar.App.Entities.csproj", "Avatar.App.Entities/"]
COPY ["Avatar.App.Service/Avatar.App.Service.csproj", "Avatar.App.Service/"]
RUN dotnet restore "Avatar.App.Api/Avatar.App.Api.csproj"
COPY . .
WORKDIR "/src/Avatar.App.Api"
RUN dotnet build "Avatar.App.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Avatar.App.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Avatar.App.Api.dll"]
