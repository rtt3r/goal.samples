using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Commands.Customers
{
    public abstract class CustomerCommand<T> : CustomerCommand, ICommand<T>
        where T : ICommandResult
    {
    }
}
