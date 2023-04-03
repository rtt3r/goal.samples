using System.Linq.Expressions;
using Goal.Seedwork.Infra.Crosscutting.Collections;
using Goal.Seedwork.Infra.Crosscutting.Extensions;
using Goal.Seedwork.Infra.Data.Query;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Goal.Samples.CQRS.Infra.Data.Query
{
    public abstract class RavenQueryRepository<TEntity> : QueryRepository<TEntity, string>
        where TEntity : class
    {
        protected IAsyncDocumentSession dbSession;
        private bool disposed;

        protected RavenQueryRepository(IAsyncDocumentSession dbSession)
        {
            this.dbSession = dbSession;
        }

        public override async Task<TEntity> LoadAsync(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            return await dbSession
                .LoadAsync<TEntity>(id, cancellationToken);
        }

        public override async Task<ICollection<TEntity>> QueryAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await dbSession
                .Query<TEntity>()
                .ToListAsync(cancellationToken);
        }

        public override async Task<IPagedCollection<TEntity>> QueryAsync(IPageSearch pageSearch, CancellationToken cancellationToken = new CancellationToken())
        {
            var query = dbSession
                .Query<TEntity>()
                .Statistics(out QueryStatistics stats);

            if (!string.IsNullOrWhiteSpace(pageSearch.SortBy))
            {
                query = OrderingHelper(query, pageSearch.SortBy, pageSearch.SortDirection, false);
            }

            query = query
                .Skip(pageSearch.PageIndex * pageSearch.PageSize)
                .Take(pageSearch.PageSize);

            return new PagedList<TEntity>(
                await query.ToListAsync(),
                stats.TotalResults);
        }

        public override async Task StoreAsync(string id, TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        {
            await dbSession.StoreAsync(entity, id, cancellationToken);
            await dbSession.SaveChangesAsync(cancellationToken);
        }

        public override async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        {
            dbSession.Delete(entity);
            await dbSession.SaveChangesAsync(cancellationToken);
        }

        public override async Task RemoveAsync(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            dbSession.Delete(id);
            await dbSession.SaveChangesAsync(cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dbSession.Dispose();
                    dbSession = null;
                }

                disposed = true;
            }
        }

        private static IRavenQueryable<T> OrderingHelper<T>(IRavenQueryable<T> source, string fieldName, SortDirection direction, bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);
            MemberExpression property = null;
            LambdaExpression sort = null;

            foreach (string propName in fieldName.Split('.'))
            {
                property = Expression.Property((Expression)property ?? param, propName);
                sort = Expression.Lambda(property, param);
            }

            string level = !anotherLevel ? "OrderBy" : "ThenBy";
            string sortDirection = direction == SortDirection.Desc ? "Descending" : string.Empty;

            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                $"{level}{sortDirection}",
                new[] { typeof(T), property?.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IRavenQueryable<T>)source.Provider.CreateQuery<T>(call);
        }
    }
}
