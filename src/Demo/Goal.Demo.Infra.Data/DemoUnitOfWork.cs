using Goal.Infra.Data.Seedwork;

namespace Goal.Demo.Infra.Data
{
    public sealed class DemoUnitOfWork : UnitOfWork
    {
        public DemoUnitOfWork(DemoContext context) : base(context)
        {
        }
    }
}
