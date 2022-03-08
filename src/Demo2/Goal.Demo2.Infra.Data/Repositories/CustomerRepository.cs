using Goal.Demo2.Domain.Aggregates.Customers;
using Goal.Infra.Data.Seedwork;
using Microsoft.EntityFrameworkCore;

namespace Goal.Demo2.Infra.Data.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
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
