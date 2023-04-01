export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations add "$1" --startup-project ../Goal.Samples.CQRS.Api/Goal.Samples.CQRS.Api.csproj --context $2DbContext --output-dir Migrations/$2
