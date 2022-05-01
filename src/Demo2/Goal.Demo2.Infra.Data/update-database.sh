export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef database update --startup-project ../Goal.Demo2/Goal.Demo2.csproj --context $1 -- --environment Migrations
