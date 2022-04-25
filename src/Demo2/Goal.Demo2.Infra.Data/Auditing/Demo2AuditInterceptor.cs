using Goal.Infra.Data.Seedwork.Auditing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;

namespace Goal.Demo2.Infra.Data.Auditing
{
    public class Demo2AuditChangesInterceptor : AuditChangesInterceptor
    {
        private readonly IAsyncDocumentSession asyncDocSession;
        private readonly IDocumentSession docSession;

        public Demo2AuditChangesInterceptor(
            IHttpContextAccessor httpContextAccessor,
            ILogger<Demo2AuditChangesInterceptor> logger,
            IAsyncDocumentSession asyncDocSession,
            IDocumentSession docSession)
            : base(httpContextAccessor, logger)
        {
            this.asyncDocSession = asyncDocSession;
            this.docSession = docSession;
        }

        protected override void SaveAuditChanges(Audit audit)
        {
            base.SaveAuditChanges(audit);

            docSession.Store(audit, audit.Id);
            docSession.SaveChanges();
        }

        protected override async Task SaveAuditChangesAsync(Audit audit, CancellationToken cancellationToken = new CancellationToken())
        {
            await base.SaveAuditChangesAsync(audit, cancellationToken);

            await asyncDocSession.StoreAsync(audit, audit.Id, cancellationToken);
            await asyncDocSession.SaveChangesAsync(cancellationToken);
        }
    }
}
