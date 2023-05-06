using Goal.Samples.DDD.Domain.People.Aggregates;
using Goal.Seedwork.Infra.Crosscutting;
using Goal.Seedwork.Infra.Crosscutting.Collections;
using Goal.Seedwork.Infra.Crosscutting.Extensions;
using Goal.Seedwork.Infra.Crosscutting.Specifications;
using Goal.Seedwork.Infra.Data;
using Goal.Seedwork.Infra.Data.Extensions.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.DDD.Infra.Data.Repositories;

public class PersonRepository : Repository<Person, string>, IPersonRepository
{
    public PersonRepository(DddDbContext context)
        : base(context)
    {
    }

    public override Person Load(string key)
    {
        return Context
            .Set<Person>()
            .Include(p => p.Cpf)
            .FirstOrDefault(p => p.Id == key);
    }

    public override ICollection<Person> Query()
    {
        return Context
            .Set<Person>()
            .Include(p => p.Cpf)
            .ToList();
    }

    public override IPagedCollection<Person> Query(IPageSearch pageSearch)
    {
        return Context
            .Set<Person>()
            .Include(p => p.Cpf)
            .ToPagedList(pageSearch);
    }

    public override ICollection<Person> Query(ISpecification<Person> specification)
    {
        Ensure.Argument.IsNotNull(specification, nameof(specification));

        return Context
            .Set<Person>()
            .Include(p => p.Cpf)
            .Where(specification.SatisfiedBy())
            .ToList();
    }

    public override IPagedCollection<Person> Query(ISpecification<Person> specification, IPageSearch pageSearch)
    {
        Ensure.Argument.IsNotNull(specification, nameof(specification));
        Ensure.Argument.IsNotNull(pageSearch, nameof(pageSearch));

        return Context
            .Set<Person>()
            .Include(p => p.Cpf)
            .Where(specification.SatisfiedBy())
            .ToPagedList(pageSearch);
    }

    public override async Task<Person> LoadAsync(string key, CancellationToken cancellationToken = default)
    {
        return await Context
            .Set<Person>()
            .Include(p => p.Cpf)
            .FirstOrDefaultAsync(p => p.Id == key, cancellationToken);
    }

    public override async Task<ICollection<Person>> QueryAsync(CancellationToken cancellationToken = default)
    {
        return await Context
            .Set<Person>()
            .Include(p => p.Cpf)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IPagedCollection<Person>> QueryAsync(IPageSearch pageSearch, CancellationToken cancellationToken = default)
    {
        return await Context
            .Set<Person>()
            .Include(p => p.Cpf)
            .ToPagedListAsync(pageSearch, cancellationToken);
    }

    public override async Task<ICollection<Person>> QueryAsync(ISpecification<Person> specification, CancellationToken cancellationToken = default)
    {
        Ensure.Argument.IsNotNull(specification, nameof(specification));

        return await Context
            .Set<Person>()
            .Include(p => p.Cpf)
            .Where(specification.SatisfiedBy())
            .ToListAsync(cancellationToken);
    }

    public override async Task<IPagedCollection<Person>> QueryAsync(ISpecification<Person> specification, IPageSearch pageSearch, CancellationToken cancellationToken = default)
    {
        Ensure.Argument.IsNotNull(specification, nameof(specification));
        Ensure.Argument.IsNotNull(pageSearch, nameof(pageSearch));

        return await Context
            .Set<Person>()
            .Include(p => p.Cpf)
            .Where(specification.SatisfiedBy())
            .ToPagedListAsync(pageSearch, cancellationToken);
    }
}
