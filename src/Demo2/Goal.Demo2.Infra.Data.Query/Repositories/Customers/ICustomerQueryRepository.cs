using Goal.Demo2.Model.Customers;
using Goal.Seedwork.Infra.Data.Query;

namespace Goal.Demo2.Infra.Data.Query.Repositories.Customers
{
    public interface ICustomerQueryRepository : IQueryRepository<CustomerModel, string>
    {
    }
}
