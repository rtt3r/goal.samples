using Goal.Seedwork.Infra.Crosscutting.Specifications;

namespace Goal.Samples.DDD.Domain.People.Aggregates;

public static class PersonSpecifications
{
    public static Specification<Person> MatchCpf(string cpf)
        => new DirectSpecification<Person>(p => p.Cpf.Number == cpf);

    public static Specification<Person> MatchId(string personId)
        => new DirectSpecification<Person>(p => p.Id == personId);
}
