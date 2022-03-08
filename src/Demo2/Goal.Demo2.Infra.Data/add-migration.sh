dotnet ef migrations add "$1" --startup-project ../Goal.Demo2.Api/Goal.Demo2.Api.csproj --context $2 --output-dir Migrations/$2
