export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef database update --startup-project ../Goal.Samples.DDD.$1/Goal.Samples.DDD.$1.csproj --context $2DbContext
