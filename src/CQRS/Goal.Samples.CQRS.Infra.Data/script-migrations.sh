export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations script --startup-project ../Goal.Samples.CQRS.Api/Goal.Samples.CQRS.Api.csproj --context $1
