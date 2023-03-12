#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["nuget.config", "/"]
COPY ["src/Infra/Ritter.Starter.Infra.Crosscutting/Ritter.Starter.Infra.Crosscutting.csproj", "src/Infra/Ritter.Starter.Infra.Crosscutting/"]
COPY ["src/Identity/Ritter.Starter.Identity/Ritter.Starter.Identity.csproj", "src/Identity/Ritter.Starter.Identity/"]

RUN dotnet restore "src/Identity/Ritter.Starter.Identity/Ritter.Starter.Identity.csproj"
COPY . .
WORKDIR "/src/src/Identity/Ritter.Starter.Identity"
RUN dotnet build "Ritter.Starter.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ritter.Starter.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ritter.Starter.Identity.dll"]
