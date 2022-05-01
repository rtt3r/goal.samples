using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Customers.Commands
{
    public abstract class CustomerCommand<T> : CustomerCommand, ICommand<T>
        where T : ICommandResult
    {
    }
}
