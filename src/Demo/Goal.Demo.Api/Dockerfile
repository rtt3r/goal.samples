#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/DDDDemo/Goal.Demo.Api/Goal.Demo.Api.csproj", "src/DDDDemo/Goal.Demo.Api/"]
COPY ["src/DDDDemo/Goal.Demo.Application/Goal.Demo.Application.csproj", "src/DDDDemo/Goal.Demo.Application/"]
COPY ["src/DDDDemo/Goal.Demo.Application.DTO/Goal.Demo.Application.DTO.csproj", "src/DDDDemo/Goal.Demo.Application.DTO/"]
COPY ["src/DDDDemo/Goal.Demo.Domain/Goal.Demo.Domain.csproj", "src/DDDDemo/Goal.Demo.Domain/"]
COPY ["src/DDDDemo/Goal.Demo.Infra.Data/Goal.Demo.Infra.Data.csproj", "src/DDDDemo/Goal.Demo.Infra.Data/"]
COPY ["src/DDDDemo/Goal.Demo.IoC/Goal.Demo.IoC.csproj", "src/DDDDemo/Goal.Demo.IoC/"]
COPY ["src/Seedwork/Domain.Seedwork/Domain.Seedwork.csproj", "src/Seedwork/Domain.Seedwork/"]
COPY ["src/Seedwork/Infra.Crosscutting/Infra.Crosscutting.csproj", "src/Seedwork/Infra.Crosscutting/"]
COPY ["src/Seedwork/Infra.Data.Seedwork/Infra.Data.Seedwork.csproj", "src/Seedwork/Infra.Data.Seedwork/"]
COPY ["src/Seedwork/Infra.Data.Query.Seedwork/Infra.Data.Query.Seedwork.csproj", "src/Seedwork/Infra.Data.Query.Seedwork/"]
COPY ["src/Seedwork/Infra.Http.Seedwork/Infra.Http.Seedwork.csproj", "src/Seedwork/Infra.Http.Seedwork/"]
COPY ["src/Seedwork/Application.Seedwork/Application.Seedwork.csproj", "src/Seedwork/Application.Seedwork/"]

RUN dotnet restore "src/DDDDemo/Goal.Demo.Api/Goal.Demo.Api.csproj"
COPY . .
WORKDIR "/src/src/DDDDemo/Goal.Demo.Api"
RUN dotnet build "Goal.Demo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Goal.Demo.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Goal.Demo.Api.dll"]
