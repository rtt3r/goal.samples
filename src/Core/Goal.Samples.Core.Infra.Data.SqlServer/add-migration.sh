export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations add "$1" --startup-project ../Goal.Samples.Core.$2/Goal.Samples.Core.$2.csproj --context SqlServer$3DbContext --output-dir Migrations/$3
