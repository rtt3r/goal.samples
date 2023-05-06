using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.DDD.Domain.People.Aggregates;

public class Person : Entity<string>
{
    public Name Name { get; private set; }
    public Document Cpf { get; private set; }

    protected Person()
        : base()
    {
    }

    public Person(Name name, Document cpf)
        : this()
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Cpf = cpf;
    }

    public static Person CreatePerson(string firstName, string lastName, string cpf)
    {
        return new Person(
            Name.CreateName(firstName, lastName),
            Document.CreateCpf(cpf));
    }
}
