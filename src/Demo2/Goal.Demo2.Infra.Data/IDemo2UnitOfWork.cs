using Goal.Demo2.Domain.Customers.Aggregates;
using Goal.Seedwork.Domain;

namespace Goal.Demo2.Infra.Data
{
    public interface IDemo2UnitOfWork : IUnitOfWork
    {
        ICustomerRepository Customers { get; }
    }
}
