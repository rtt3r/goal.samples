using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class Permission : Entity<string>
{
    public string OperationId { get; private set; }
    public string ResourceId { get; private set; }
    public Operation Operation { get; private set; }
    public Resource Resource { get; private set; }
    public ICollection<Authorization> Authorizations { get; private set; }

    protected Permission()
    {
        Id = Guid.NewGuid().ToString();
        Authorizations = new HashSet<Authorization>();
    }
}
