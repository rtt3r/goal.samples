using Goal.Seedwork.Infra.Data;

namespace Goal.Demo.Infra.Data
{
    public sealed class DemoUnitOfWork : UnitOfWork
    {
        public DemoUnitOfWork(DemoContext context) : base(context)
        {
        }
    }
}
