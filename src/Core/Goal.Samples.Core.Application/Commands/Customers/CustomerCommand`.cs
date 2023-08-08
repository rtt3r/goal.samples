using Goal.Seedwork.Application.Commands;

namespace Goal.Samples.Core.Application.Commands.Customers;

public abstract class CustomerCommand<T> : CustomerCommand, ICommand<T>
    where T : ICommandResult
{
}
