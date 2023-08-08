using Goal.Samples.Core.Model.Customers;
using Goal.Samples.Infra.Data.Query;
using Raven.Client.Documents.Session;

namespace Goal.Samples.Core.Infra.Data.Query.Repositories.Customers;

public class CustomerQueryRepository : RavenQueryRepository<Customer>, ICustomerQueryRepository
{
    public CustomerQueryRepository(IAsyncDocumentSession dbSession)
        : base(dbSession)
    {
    }
}
