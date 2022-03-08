using Goal.Infra.Data.Seedwork;

namespace Goal.Demo2.Infra.Data
{
    public sealed class Demo2UnitOfWork : UnitOfWork
    {
        public Demo2UnitOfWork(Demo2Context context) : base(context)
        {
        }
    }
}
