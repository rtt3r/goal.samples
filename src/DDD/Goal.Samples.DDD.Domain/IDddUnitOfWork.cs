using Goal.Samples.DDD.Domain.People.Aggregates;
using Goal.Seedwork.Domain;

namespace Goal.Samples.DDD.Domain;

public interface IDddUnitOfWork : IUnitOfWork
{
    IPeopleRepository People { get; }
}
