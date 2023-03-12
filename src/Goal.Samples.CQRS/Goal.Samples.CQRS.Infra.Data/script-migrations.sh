export ASPNETCORE_ENVIRONMENT=Migrations
dotnet ef migrations script --startup-project ../Goal.Samples.CQRS/Goal.Samples.CQRS.csproj --context Demo2Context
