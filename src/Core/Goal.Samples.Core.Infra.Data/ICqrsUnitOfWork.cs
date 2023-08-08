using Goal.Samples.Core.Domain.Customers.Aggregates;
using Goal.Seedwork.Domain;

namespace Goal.Samples.Core.Infra.Data;

public interface ICqrsUnitOfWork : IUnitOfWork
{
    ICustomerRepository Customers { get; }
}
