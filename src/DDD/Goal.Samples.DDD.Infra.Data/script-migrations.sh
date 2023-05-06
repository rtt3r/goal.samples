export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations script --startup-project ../Goal.Samples.DDD.$1/Goal.Samples.DDD.$1.csproj --context $2DbContext
