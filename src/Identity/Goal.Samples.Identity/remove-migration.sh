export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations remove --context $1DbContext
