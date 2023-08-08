using Goal.Samples.Core.Model.Customers;
using Goal.Seedwork.Infra.Data.Query;

namespace Goal.Samples.Core.Infra.Data.Query.Repositories.Customers;

public interface ICustomerQueryRepository : IQueryRepository<Customer, string>
{
}
