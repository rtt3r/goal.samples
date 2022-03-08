using Goal.Demo2.Dto.Customers;
using Goal.Infra.Data.Query.Seedwork;

namespace Goal.Demo2.Infra.Data.Query.Repositories.Customers
{
    public interface ICustomerQueryRepository : IQueryRepository<CustomerDto, string>
    {
    }
}
