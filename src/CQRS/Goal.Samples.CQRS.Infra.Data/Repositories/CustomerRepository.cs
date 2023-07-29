using Goal.Samples.CQRS.Domain.Customers.Aggregates;
using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.Repositories;

public class CustomerRepository : Repository<Customer, string>, ICustomerRepository
{
    public CustomerRepository(CqrsDbContext context)
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
