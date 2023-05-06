using Goal.Samples.DDD.Domain;
using Goal.Samples.DDD.Domain.People.Aggregates;
using Goal.Seedwork.Infra.Data;

namespace Goal.Samples.DDD.Infra.Data;

public sealed class DddUnitOfWork : UnitOfWork, IDddUnitOfWork
{
    public DddUnitOfWork(
        DddDbContext context,
        IPeopleRepository peopleRepository)
        : base(context)
    {
        People = peopleRepository;
    }

    public IPeopleRepository People { get; }
}
