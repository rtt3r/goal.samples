export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef database update --startup-project ../Goal.Samples.Core.$1/Goal.Samples.Core.$1.csproj --context $2DbContext
