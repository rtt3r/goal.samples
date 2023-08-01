using Goal.Samples.CQRS.Model.Customers;
using Goal.Samples.Infra.Data.Query;
using Raven.Client.Documents.Session;

namespace Goal.Samples.CQRS.Infra.Data.Query.Repositories.Customers;

public class CustomerQueryRepository : RavenQueryRepository<Customer>, ICustomerQueryRepository
{
    public CustomerQueryRepository(IAsyncDocumentSession dbSession)
        : base(dbSession)
    {
    }
}
