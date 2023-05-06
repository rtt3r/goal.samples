using Goal.Samples.DDD.Domain.People.Aggregates;
using Goal.Seedwork.Infra.Data;

namespace Goal.Samples.DDD.Infra.Data;

public sealed class DddUnitOfWork : UnitOfWork, IDddUnitOfWork
{
    public DddUnitOfWork(
        DddDbContext context,
        IPersonRepository personRepository)
        : base(context)
    {
        People = personRepository;
    }

    public IPersonRepository People { get; }
}
