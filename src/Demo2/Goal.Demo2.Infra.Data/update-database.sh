export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef database update --startup-project ../Goal.Demo2.Api/Goal.Demo2.Api.csproj --context $1 -- --environment Migrations
