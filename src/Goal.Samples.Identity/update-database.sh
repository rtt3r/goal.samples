export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef database update --context $1DbContext -- --environment Migrations
