namespace Goal.Samples.Identity.Models;

public class Permission
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

    public string Id { get; set; }
    public string ResourceId { get; set; }
    public string OperationId { get; set; }
    public Resource Resource { get; set; }
    public Operation Operation { get; set; }
    public IEnumerable<Authorization> Authorizations { get; set; } = new List<Authorization>();
}
