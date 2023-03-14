using Goal.Seedwork.Application.Commands;

namespace Goal.Samples.CQRS.Api.Application.Commands.Customers
{
    public abstract class CustomerCommand<T> : CustomerCommand, ICommand<T>
        where T : ICommandResult
    {
    }
}
