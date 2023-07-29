using Goal.Samples.CQRS.Domain.Customers.Aggregates;
using Goal.Seedwork.Infra.Data;

namespace Goal.Samples.CQRS.Infra.Data;

public sealed class CqrsUnitOfWork : UnitOfWork, ICqrsUnitOfWork
{
    public CqrsUnitOfWork(
        CqrsDbContext context,
        ICustomerRepository customerRepository)
        : base(context)
    {
        Customers = customerRepository;
    }

    public ICustomerRepository Customers { get; }
}
