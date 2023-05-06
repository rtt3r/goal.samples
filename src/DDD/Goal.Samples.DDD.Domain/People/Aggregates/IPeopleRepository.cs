using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.DDD.Domain.People.Aggregates;

public interface IPeopleRepository : IRepository<Person, string>
{
}
