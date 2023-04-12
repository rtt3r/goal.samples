using Goal.Seedwork.Domain.Aggregates;

namespace Goal.Samples.CQRS.Domain.Security;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<ICollection<Permission>> FindInRoles(IEnumerable<string> roles);
}
