using System.Diagnostics;
using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.DDD.Domain.People.Aggregates;

[DebuggerDisplay("Full Name = {FullName()}")]
public class Name : ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    protected Name()
        : base()
    {
    }

    public Name(string firstName, string lastName)
        : this()
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FullName()
        => $"{FirstName} {LastName}".Trim();

    public static Name CreateName(string firstName, string lastName)
        => new(firstName, lastName);
}
