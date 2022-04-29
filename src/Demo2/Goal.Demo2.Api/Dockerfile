#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["nuget.config", "/"]
COPY ["src/Demo2/Goal.Demo2.Api/Goal.Demo2.Api.csproj", "src/Demo2/Goal.Demo2.Api/"]
COPY ["src/Demo2/Goal.Demo2.Infra.Data/Goal.Demo2.Infra.Data.csproj", "src/Demo2/Goal.Demo2.Infra.Data/"]
COPY ["src/Demo2/Goal.Demo2.Infra.Data.Query/Goal.Demo2.Infra.Data.Query.csproj", "src/Demo2/Goal.Demo2.Infra.Data.Query/"]
COPY ["src/Demo2/Goal.Demo2.Domain/Goal.Demo2.Domain.csproj", "src/Demo2/Goal.Demo2.Domain/"]
COPY ["src/Demo2/Goal.Demo2.Dto/Goal.Demo2.Dto.csproj", "src/Demo2/Goal.Demo2.Dto/"]
COPY ["src/Demo2/Goal.Demo2.Infra.Crosscutting/Goal.Demo2.Infra.Crosscutting.csproj", "src/Demo2/Goal.Demo2.Infra.Crosscutting.Dto/"]

RUN dotnet restore "src/Demo2/Goal.Demo2.Api/Goal.Demo2.Api.csproj"
COPY . .
WORKDIR "/src/src/Demo2/Goal.Demo2.Api"
RUN dotnet build "Goal.Demo2.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Goal.Demo2.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Goal.Demo2.Api.dll"]
