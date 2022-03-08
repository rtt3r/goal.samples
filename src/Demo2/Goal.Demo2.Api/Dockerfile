#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/Demo2/Goal.Demo2.Api/Goal.Demo2.Api.csproj", "src/Demo2/Goal.Demo2.Api/"]
COPY ["src/Demo2/Goal.Demo2.Infra.Data/Goal.Demo2.Infra.Data.csproj", "src/Demo2/Goal.Demo2.Infra.Data/"]
COPY ["src/Demo2/Goal.Demo2.Infra.Data.Query/Goal.Demo2.Infra.Data.Query.csproj", "src/Demo2/Goal.Demo2.Infra.Data.Query/"]
COPY ["src/Demo2/Goal.Demo2.Domain/Goal.Demo2.Domain.csproj", "src/Demo2/Goal.Demo2.Domain/"]
COPY ["src/Demo2/Goal.Demo2.Dto/Goal.Demo2.Dto.csproj", "src/Demo2/Goal.Demo2.Dto/"]
COPY ["src/Seedwork/Domain.Seedwork/Domain.Seedwork.csproj", "src/Seedwork/Domain.Seedwork/"]
COPY ["src/Seedwork/Infra.Crosscutting/Infra.Crosscutting.csproj", "src/Seedwork/Infra.Crosscutting/"]
COPY ["src/Seedwork/Infra.Data.Seedwork/Infra.Data.Seedwork.csproj", "src/Seedwork/Infra.Data.Seedwork/"]
COPY ["src/Seedwork/Infra.Data.Query.Seedwork/Infra.Data.Query.Seedwork.csproj", "src/Seedwork/Infra.Data.Query.Seedwork/"]
COPY ["src/Seedwork/Infra.Http.Seedwork/Infra.Http.Seedwork.csproj", "src/Seedwork/Infra.Http.Seedwork/"]
COPY ["src/Seedwork/Application.Seedwork/Application.Seedwork.csproj", "src/Seedwork/Application.Seedwork/"]

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
