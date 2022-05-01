export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations script --startup-project ../Goal.Demo2/Goal.Demo2.csproj --context Demo2Context
