#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["nuget.config", "/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Infra.Crosscutting/Goal.Samples.CQRS.Infra.Crosscutting.csproj", "Goal.Samples.CQRS.Infra.Crosscutting.Dto/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Worker/Goal.Samples.CQRS.Worker.csproj", "Goal.Samples.CQRS.Worker/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Application/Goal.Samples.CQRS.Application.csproj", "Goal.Samples.CQRS.Application/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Model/Goal.Samples.CQRS.Model.csproj", "Goal.Samples.CQRS.Model/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Domain/Goal.Samples.CQRS.Domain.csproj", "Goal.Samples.CQRS.Domain/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Infra.Data/Goal.Samples.CQRS.Infra.Data.csproj", "Goal.Samples.CQRS.Infra.Data/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Infra.Data.Query/Goal.Samples.CQRS.Infra.Data.Query.csproj", "Goal.Samples.CQRS.Infra.Data.Query/"]
COPY ["src/Goal.Samples.CQRS/Goal.Samples.CQRS.Infra.IoC/Goal.Samples.CQRS.Infra.IoC.csproj", "Goal.Samples.CQRS.Infra.IoC/"]

RUN dotnet restore "src/Goal.Samples.CQRS/Goal.Samples.CQRS.Worker/Goal.Samples.CQRS.Worker.csproj"

COPY . .
WORKDIR "/src/Goal.Samples.CQRS.Worker"
RUN dotnet build "Goal.Samples.CQRS.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Goal.Samples.CQRS.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Goal.Samples.CQRS.Worker.dll"]
