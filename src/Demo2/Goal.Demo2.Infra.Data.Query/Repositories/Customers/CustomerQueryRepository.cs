using Goal.Demo2.Dto.Customers;
using Raven.Client.Documents.Session;

namespace Goal.Demo2.Infra.Data.Query.Repositories.Customers
{
    public class CustomerQueryRepository : RavenQueryRepository<CustomerDto>, ICustomerQueryRepository
    {
        public CustomerQueryRepository(IAsyncDocumentSession dbSession)
            : base(dbSession)
        {
        }
    }
}
