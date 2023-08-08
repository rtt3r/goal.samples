using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.Core.Domain.Customers.Aggregates;

public interface ICustomerRepository : IRepository<Customer, string>
{
    Task<Customer> GetByEmail(string email);
}
