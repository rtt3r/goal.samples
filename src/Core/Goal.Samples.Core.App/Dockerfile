#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["nuget.config", "."]
COPY ["src/CQRS/Goal.Samples.Core.App/Goal.Samples.Core.App.csproj", "src/CQRS/Goal.Samples.Core.App/"]
COPY ["src/Infra/Goal.Samples.Infra.Http/Goal.Samples.Infra.Http.csproj", "src/Infra/Goal.Samples.Infra.Http/"]
COPY ["src/Infra/Goal.Samples.Infra.Crosscutting/Goal.Samples.Infra.Crosscutting.csproj", "src/Infra/Goal.Samples.Infra.Crosscutting/"]
COPY ["src/Infra/Goal.Samples.Infra.Data.Query/Goal.Samples.Infra.Data.Query.csproj", "src/Infra/Goal.Samples.Infra.Data.Query/"]
COPY ["src/CQRS/Goal.Samples.Core.Application/Goal.Samples.Core.Application.csproj", "src/CQRS/Goal.Samples.Core.Application/"]
COPY ["src/CQRS/Goal.Samples.Core.Domain/Goal.Samples.Core.Domain.csproj", "src/CQRS/Goal.Samples.Core.Domain/"]
COPY ["src/CQRS/Goal.Samples.Core.Infra.Data.Query/Goal.Samples.Core.Infra.Data.Query.csproj", "src/CQRS/Goal.Samples.Core.Infra.Data.Query/"]
COPY ["src/CQRS/Goal.Samples.Core.Model/Goal.Samples.Core.Model.csproj", "src/CQRS/Goal.Samples.Core.Model/"]
COPY ["src/CQRS/Goal.Samples.Core.Infra.Data/Goal.Samples.Core.Infra.Data.csproj", "src/CQRS/Goal.Samples.Core.Infra.Data/"]
COPY ["src/CQRS/Goal.Samples.Core.Infra.IoC/Goal.Samples.Core.Infra.IoC.csproj", "src/CQRS/Goal.Samples.Core.Infra.IoC/"]

RUN dotnet restore "src/CQRS/Goal.Samples.Core.App/Goal.Samples.Core.App.csproj"

COPY . .
WORKDIR "/src/src/CQRS/Goal.Samples.Core.App"
RUN dotnet build "Goal.Samples.Core.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Goal.Samples.Core.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Goal.Samples.Core.App.dll"]
