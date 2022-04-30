using Goal.Demo2.Domain.Aggregates.Customers;
using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Goal.Demo2.Infra.Data.Repositories
{
    public class CustomerRepository : Repository<Customer, string>, ICustomerRepository
    {
        public CustomerRepository(Demo2Context context)
            : base(context)
        {
        }

        public async Task<Customer> GetByEmail(string email)
        {
            return await Context
                .Set<Customer>()
                .FirstOrDefaultAsync(p => p.Email == email);
        }
    }
}
