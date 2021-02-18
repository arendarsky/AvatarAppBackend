#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Avatar.App.Api/Avatar.App.Api.csproj", "Avatar.App.Api/"]
COPY ["Avatar.App.Final/Avatar.App.Final.csproj", "Avatar.App.Final/"]
COPY ["Avatar.App.SharedKernel/Avatar.App.SharedKernel.csproj", "Avatar.App.SharedKernel/"]
COPY ["Avatar.App.Authentication/Avatar.App.Authentication.csproj", "Avatar.App.Authentication/"]
COPY ["Avatar.App.Administration/Avatar.App.Administration.csproj", "Avatar.App.Administration/"]
COPY ["Avatar.App.Schedulers/Avatar.App.Schedulers.csproj", "Avatar.App.Schedulers/"]
COPY ["Avatar.App.Casting/Avatar.App.Casting.csproj", "Avatar.App.Casting/"]
COPY ["Avatar.App.Content/Avatar.App.Content.csproj", "Avatar.App.Content/"]
COPY ["Avatar.App.Communications/Avatar.App.Communications.csproj", "Avatar.App.Communications/"]
COPY ["Avatar.App.Infrastructure/Avatar.App.Infrastructure.csproj", "Avatar.App.Infrastructure/"]
COPY ["Avatar.App.Rating/Avatar.App.Rating.csproj", "Avatar.App.Rating/"]
COPY ["Avatar.App.Semifinal/Avatar.App.Semifinal.csproj", "Avatar.App.Semifinal/"]
COPY ["Avatar.App.Profile/Avatar.App.Profile.csproj", "Avatar.App.Profile/"]
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