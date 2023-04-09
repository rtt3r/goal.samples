export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations script --context $1DbContext -- --environment Migrations
