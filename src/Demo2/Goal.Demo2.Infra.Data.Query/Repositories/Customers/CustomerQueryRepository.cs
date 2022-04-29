using Goal.Demo2.Model.Customers;
using Raven.Client.Documents.Session;

namespace Goal.Demo2.Infra.Data.Query.Repositories.Customers
{
    public class CustomerQueryRepository : RavenQueryRepository<CustomerModel>, ICustomerQueryRepository
    {
        public CustomerQueryRepository(IAsyncDocumentSession dbSession)
            : base(dbSession)
        {
        }
    }
}
