# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1

# We use the alpine version since it's easier to install NPM there (as compared to debian)
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
RUN apk add --update npm
WORKDIR /app
EXPOSE 80
EXPOSE 443

# We also need NPM for the build.
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
RUN apk add --update npm
WORKDIR /src
COPY ["FoodFile.fsproj", ""]
RUN dotnet restore "./FoodFile.fsproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FoodFile.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodFile.fsproj" -c Release -o /app/publish

FROM base AS final
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "FoodFile.dll"]