using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Demo2.Domain.Customers.Aggregates
{
    public interface ICustomerRepository : IRepository<Customer, string>
    {
        Task<Customer> GetByEmail(string email);
    }
}
