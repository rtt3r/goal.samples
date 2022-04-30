using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Demo2.Domain.Aggregates.Customers
{
    public interface ICustomerRepository : IRepository<Customer, string>
    {
        Task<Customer> GetByEmail(string email);
    }
}