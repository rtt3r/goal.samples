using Goal.Samples.CQRS.Model.Customers;
using Goal.Seedwork.Infra.Data.Query;

namespace Goal.Samples.CQRS.Infra.Data.Query.Repositories.Customers
{
    public interface ICustomerQueryRepository : IQueryRepository<CustomerModel, string>
    {
    }
}
