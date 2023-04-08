using Goal.Samples.CQRS.Domain.Customers.Aggregates;
using Goal.Seedwork.Domain;

namespace Goal.Samples.CQRS.Infra.Data
{
    public interface ISampleUnitOfWork : IUnitOfWork
    {
        ICustomerRepository Customers { get; }
    }
}
