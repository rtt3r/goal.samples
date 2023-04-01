using Goal.Samples.CQRS.Domain.Customers.Aggregates;
using Goal.Seedwork.Infra.Data;

namespace Goal.Samples.CQRS.Infra.Data
{
    public sealed class SampleUnitOfWork : UnitOfWork, ISampleUnitOfWork
    {
        public SampleUnitOfWork(
            SampleDbContext context,
            ICustomerRepository customerRepository)
            : base(context)
        {
            Customers = customerRepository;
        }

        public ICustomerRepository Customers { get; }
    }
}
