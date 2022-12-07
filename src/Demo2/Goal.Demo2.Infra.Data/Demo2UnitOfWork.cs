using Goal.Demo2.Domain.Customers.Aggregates;
using Goal.Seedwork.Infra.Data;

namespace Goal.Demo2.Infra.Data
{
    public sealed class Demo2UnitOfWork : UnitOfWork, IDemo2UnitOfWork
    {
        public Demo2UnitOfWork(
            Demo2Context context,
            ICustomerRepository customerRepository)
            : base(context)
        {
            Customers = customerRepository;
        }

        public ICustomerRepository Customers { get; }
    }
}
