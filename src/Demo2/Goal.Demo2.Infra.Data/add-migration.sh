export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations add "$1" --startup-project ../Goal.Demo2/Goal.Demo2.csproj --context $2 --output-dir Migrations/$2
