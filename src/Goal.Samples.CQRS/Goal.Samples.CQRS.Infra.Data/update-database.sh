export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef database update --startup-project ../Goal.Samples.CQRS/Goal.Samples.CQRS.csproj --context $1 -- --environment Migrations
