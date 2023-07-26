export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations remove --startup-project ../Goal.Samples.CQRS.$1/Goal.Samples.CQRS.$1.csproj --context $2DbContext
