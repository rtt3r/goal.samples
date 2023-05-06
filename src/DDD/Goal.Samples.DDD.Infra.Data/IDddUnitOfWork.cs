using Goal.Samples.DDD.Domain.People.Aggregates;
using Goal.Seedwork.Domain;

namespace Goal.Samples.DDD.Infra.Data;

public interface IDddUnitOfWork : IUnitOfWork
{
    IPersonRepository People { get; }
}
