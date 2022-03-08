using Goal.Domain.Seedwork.Commands;

namespace Goal.Demo2.Api.Application.Commands.Customers
{
    public abstract class CustomerCommand<T> : CustomerCommand, ICommand<T>
        where T : ICommandResult
    {
    }
}
