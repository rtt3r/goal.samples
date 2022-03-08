using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Goal.Demo.Domain.Aggregates.People;
using Goal.Infra.Crosscutting.Collections;
using Goal.Infra.Crosscutting.Extensions;
using Goal.Infra.Data.Seedwork;
using Microsoft.EntityFrameworkCore;

namespace Goal.Demo.Infra.Data.Repositories
{
    public class PersonRepository : Repository<Person, string>, IPersonRepository
    {
        public PersonRepository(DbContext context)
            : base(context)
        {
        }

        public override Person Load(string id)
        {
            return Context
                .Set<Person>()
                .Include(p => p.Cpf)
                .FirstOrDefault(p => p.Id == id);
        }

        public override async Task<Person> LoadAsync(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            return await Context
                .Set<Person>()
                .Include(p => p.Cpf)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public override ICollection<Person> Query()
        {
            return Context
                .Set<Person>()
                .Include(p => p.Cpf)
                .ToList();
        }

        public override async Task<ICollection<Person>> QueryAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await Context
                .Set<Person>()
                .Include(p => p.Cpf)
                .ToListAsync(cancellationToken);
        }

        public override IPagedCollection<Person> Query(IPagination pagination)
        {
            return Context
                .Set<Person>()
                .Include(p => p.Cpf)
                .PaginateList(pagination);
        }

        public override async Task<IPagedCollection<Person>> QueryAsync(IPagination pagination, CancellationToken cancellationToken = new CancellationToken())
        {
            return await Context
                .Set<Person>()
                .AsNoTracking()
                .Include(p => p.Cpf)
                .PaginateListAsync(pagination, cancellationToken);
        }
    }
}
