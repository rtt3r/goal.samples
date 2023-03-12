using Goal.Samples.CQRS.Model.Customers;
using Raven.Client.Documents.Session;

namespace Goal.Samples.CQRS.Infra.Data.Query.Repositories.Customers
{
    public class CustomerQueryRepository : RavenQueryRepository<CustomerModel>, ICustomerQueryRepository
    {
        public CustomerQueryRepository(IAsyncDocumentSession dbSession)
            : base(dbSession)
        {
        }
    }
}
