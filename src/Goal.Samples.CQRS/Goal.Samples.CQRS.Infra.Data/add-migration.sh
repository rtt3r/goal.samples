export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations add "$1" --startup-project ../Goal.Samples.CQRS/Goal.Samples.CQRS.csproj --context $2 --output-dir Migrations/$2
