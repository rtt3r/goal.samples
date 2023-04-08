export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations add "$2_$1" --context $2DbContext --output-dir Data/Migrations/$2
