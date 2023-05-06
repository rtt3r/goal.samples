export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations add "$1" --startup-project ../Goal.Samples.DDD.$2/Goal.Samples.DDD.$2.csproj --context $3DbContext --output-dir Migrations/$3
