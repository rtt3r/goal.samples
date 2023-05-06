#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/DDD/Goal.Samples.DDD.Api/Goal.Samples.DDD.Api.csproj", "src/DDD/Goal.Samples.DDD.Api/"]
COPY ["src/Infra/Goal.Samples.Infra.Http/Goal.Samples.Infra.Http.csproj", "src/Infra/Goal.Samples.Infra.Http/"]
COPY ["src/Infra/Goal.Samples.Infra.Crosscutting/Goal.Samples.Infra.Crosscutting.csproj", "src/Infra/Goal.Samples.Infra.Crosscutting/"]
COPY ["src/DDD/Goal.Samples.DDD.Application.DTO/Goal.Samples.DDD.Application.DTO.csproj", "src/DDD/Goal.Samples.DDD.Application.DTO/"]
COPY ["src/DDD/Goal.Samples.DDD.Application/Goal.Samples.DDD.Application.csproj", "src/DDD/Goal.Samples.DDD.Application/"]
COPY ["src/DDD/Goal.Samples.DDD.Domain/Goal.Samples.DDD.Domain.csproj", "src/DDD/Goal.Samples.DDD.Domain/"]
COPY ["src/DDD/Goal.Samples.DDD.Infra.IoC/Goal.Samples.DDD.Infra.IoC.csproj", "src/DDD/Goal.Samples.DDD.Infra.IoC/"]
COPY ["src/DDD/Goal.Samples.DDD.Infra.Data/Goal.Samples.DDD.Infra.Data.csproj", "src/DDD/Goal.Samples.DDD.Infra.Data/"]
RUN dotnet restore "src/DDD/Goal.Samples.DDD.Api/Goal.Samples.DDD.Api.csproj"
COPY . .
WORKDIR "/src/src/DDD/Goal.Samples.DDD.Api"
RUN dotnet build "Goal.Samples.DDD.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Goal.Samples.DDD.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Goal.Samples.DDD.Api.dll"]