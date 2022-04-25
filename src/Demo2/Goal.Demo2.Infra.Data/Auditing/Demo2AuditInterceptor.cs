using Goal.Infra.Data.Seedwork.Auditing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Goal.Demo2.Infra.Data.Auditing
{
    public class Demo2AuditChangesInterceptor : AuditChangesInterceptor
    {
        public Demo2AuditChangesInterceptor(
            IHttpContextAccessor httpContextAccessor,
            ILogger<Demo2AuditChangesInterceptor> logger)
            : base(httpContextAccessor, logger)
        {
        }

        protected override void SaveAuditChanges(Audit audit) => logger.LogInformation($"Saving audit: {audit}");
    }
}
