#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["nuget.config", "."]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Api/Goal.Samples.CQRS.Api.csproj", "src/Goal.Samples.CQRS/Goal.Samples.CQRS.Api/"]
COPY ["src/Goal.Samples.Infra/Goal.Samples.Infra.Http/Goal.Samples.Infra.Http.csproj", "src/Goal.Samples.Infra/Goal.Samples.Infra.Http/"]
COPY ["src/Goal.Samples.Infra/Goal.Samples.Infra.Crosscutting/Goal.Samples.Infra.Crosscutting.csproj", "src/Goal.Samples.Infra/Goal.Samples.Infra.Crosscutting/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Application/Goal.Samples.CQRS.Application.csproj", "src/Goal.Samples.CQRS/Goal.Samples.CQRS.Application/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Domain/Goal.Samples.CQRS.Domain.csproj", "src/Goal.Samples.CQRS/Goal.Samples.CQRS.Domain/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Infra.Data.Query/Goal.Samples.CQRS.Infra.Data.Query.csproj", "src/Goal.Samples.CQRS/Goal.Samples.CQRS.Infra.Data.Query/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Model/Goal.Samples.CQRS.Model.csproj", "src/Goal.Samples.CQRS/Goal.Samples.CQRS.Model/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Infra.Data/Goal.Samples.CQRS.Infra.Data.csproj", "src/Goal.Samples.CQRS/Goal.Samples.CQRS.Infra.Data/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Infra.IoC/Goal.Samples.CQRS.Infra.IoC.csproj", "src/Goal.Samples.CQRS/Goal.Samples.CQRS.Infra.IoC/"]

RUN dotnet restore "src/Goal.Samples.CQRS/Goal.Samples.CQRS.Api/Goal.Samples.CQRS.Api.csproj"

COPY . .
WORKDIR "/src/src/Goal.Samples.CQRS/Goal.Samples.CQRS.Api"
RUN dotnet build "Goal.Samples.CQRS.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Goal.Samples.CQRS.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Goal.Samples.CQRS.Api.dll"]
