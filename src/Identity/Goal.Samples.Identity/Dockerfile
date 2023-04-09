#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["nuget.config", "/"]
COPY ["src/Infra/Goal.Samples.Infra.Crosscutting/Goal.Samples.Infra.Crosscutting.csproj", "src/Infra/Goal.Samples.Infra.Crosscutting/"]
COPY ["src/Identity/Goal.Samples.Identity/Goal.Samples.Identity.csproj", "src/Identity/Goal.Samples.Identity/"]

RUN dotnet restore "src/Identity/Goal.Samples.Identity/Goal.Samples.Identity.csproj"
COPY . .
WORKDIR "/src/src/Identity/Goal.Samples.Identity"
RUN dotnet build "Goal.Samples.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Goal.Samples.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Goal.Samples.Identity.dll"]
