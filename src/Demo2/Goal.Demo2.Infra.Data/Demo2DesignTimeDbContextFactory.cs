using Goal.Infra.Data.Seedwork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Logging;

namespace Goal.Demo2.Infra.Data
{
    public class Demo2DesignTimeDbContextFactory : DesignTimeDbContextFactory<Demo2Context>
    {
        protected override Demo2Context CreateNewInstance(DbContextOptionsBuilder<Demo2Context> optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
            return new Demo2Context(optionsBuilder.Options);
        }
    }

    #region AuditContext

    public class AuditContextDesignTimeDbContextFactory : DesignTimeDbContextFactory<AuditContext>
    {
        protected override AuditContext CreateNewInstance(DbContextOptionsBuilder<AuditContext> optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
            return new AuditContext(optionsBuilder.Options);
        }
    }

    public class AuditContext : DbContext
    {
        private readonly string _connectionString;

        public AuditContext(DbContextOptions<AuditContext> options)
           : base(options)
        {
            _connectionString = options?.FindExtension<SqlServerOptionsExtension>()?.ConnectionString;
        }

        public AuditContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_connectionString);

        public DbSet<SaveChangesAudit> SaveChangesAudits { get; set; }
    }

    public class SaveChangesAudit
    {
        public int Id { get; set; }
        public Guid AuditId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }

        public ICollection<EntityAudit> Entities { get; } = new List<EntityAudit>();
    }

    public class EntityAudit
    {
        public int Id { get; set; }
        public EntityState State { get; set; }
        public string AuditMessage { get; set; }

        public SaveChangesAudit SaveChangesAudit { get; set; }
    }
    #endregion

    public class AuditingInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger logger;
        private readonly string _connectionString;

        private SaveChangesAudit _audit;

        protected virtual string CurrentPrincipal => httpContextAccessor?.HttpContext?.User?.Identity?.Name;

        //protected AuditChangesInterceptor(IHttpContextAccessor httpContextAccessor, ILogger logger)
        //{
        //    this.httpContextAccessor = httpContextAccessor;
        //    this.logger = logger;
        //}
        //{

        public AuditingInterceptor(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region SavingChanges
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            _audit = CreateAudit(eventData.Context);

            using var auditContext = new AuditContext(_connectionString);

            auditContext.Add(_audit);
            await auditContext.SaveChangesAsync();

            return result;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            _audit = CreateAudit(eventData.Context);

            using var auditContext = new AuditContext(_connectionString);
            auditContext.Add(_audit);
            auditContext.SaveChanges();

            return result;
        }
        #endregion

        #region SavedChanges
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            using var auditContext = new AuditContext(_connectionString);

            auditContext.Attach(_audit);
            _audit.Succeeded = true;
            _audit.EndTime = DateTime.UtcNow;

            auditContext.SaveChanges();

            return result;
        }

        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            using var auditContext = new AuditContext(_connectionString);

            auditContext.Attach(_audit);
            _audit.Succeeded = true;
            _audit.EndTime = DateTime.UtcNow;

            await auditContext.SaveChangesAsync(cancellationToken);

            return result;
        }
        #endregion

        #region SaveChangesFailed
        public override void SaveChangesFailed(DbContextErrorEventData eventData)
        {
            using var auditContext = new AuditContext(_connectionString);

            auditContext.Attach(_audit);
            _audit.Succeeded = false;
            _audit.EndTime = DateTime.UtcNow;
            _audit.ErrorMessage = eventData.Exception.Message;

            auditContext.SaveChanges();
        }

        public override async Task SaveChangesFailedAsync(
            DbContextErrorEventData eventData,
            CancellationToken cancellationToken = default)
        {
            using var auditContext = new AuditContext(_connectionString);

            auditContext.Attach(_audit);
            _audit.Succeeded = false;
            _audit.EndTime = DateTime.UtcNow;
            _audit.ErrorMessage = eventData.Exception.InnerException?.Message;

            await auditContext.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region CreateAudit
        private static SaveChangesAudit CreateAudit(DbContext context)
        {
            context.ChangeTracker.DetectChanges();

            var audit = new SaveChangesAudit { AuditId = Guid.NewGuid(), StartTime = DateTime.UtcNow };

            foreach (EntityEntry entry in context.ChangeTracker.Entries())
            {
                string auditMessage = entry.State switch
                {
                    EntityState.Deleted => CreateDeletedMessage(entry),
                    EntityState.Modified => CreateModifiedMessage(entry),
                    EntityState.Added => CreateAddedMessage(entry),
                    _ => null
                };

                if (auditMessage != null)
                {
                    audit.Entities.Add(new EntityAudit { State = entry.State, AuditMessage = auditMessage });
                }
            }

            return audit;

            string CreateAddedMessage(EntityEntry entry)
                => entry.Properties.Aggregate(
                    $"Inserting {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => auditString + $"{property.Metadata.Name}: '{property.CurrentValue}' ");

            string CreateModifiedMessage(EntityEntry entry)
                => entry.Properties.Where(property => property.IsModified || property.Metadata.IsPrimaryKey()).Aggregate(
                    $"Updating {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => auditString + $"{property.Metadata.Name}: '{property.CurrentValue}' ");

            string CreateDeletedMessage(EntityEntry entry)
                => entry.Properties.Where(property => property.Metadata.IsPrimaryKey()).Aggregate(
                    $"Deleting {entry.Metadata.DisplayName()} with ",
                    (auditString, property) => auditString + $"{property.Metadata.Name}: '{property.CurrentValue}' ");
        }
        #endregion
    }
}
