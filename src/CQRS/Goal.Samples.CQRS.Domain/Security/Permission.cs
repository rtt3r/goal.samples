using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public class Permission : Entity<string>
{
    protected Permission()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Permission(Resource resource, Operation operation)
        : this()
    {
        Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        Operation = operation ?? throw new ArgumentNullException(nameof(operation));

        ResourceId = resource.Id;
        OperationId = operation.Id;
    }

    public string ResourceId { get; protected set; }
    public string OperationId { get; protected set; }
    public Resource Resource { get; protected set; }
    public Operation Operation { get; protected set; }
    public IEnumerable<Authorization> Authorizations { get; protected set; } = new List<Authorization>();
}
