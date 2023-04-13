export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations add "$1" --startup-project ../Goal.Samples.CQRS.$2/Goal.Samples.CQRS.$2.csproj --context $3DbContext --output-dir Migrations/$3
