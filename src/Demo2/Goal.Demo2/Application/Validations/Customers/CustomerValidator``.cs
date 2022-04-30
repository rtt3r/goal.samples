using Goal.Demo2.Application.Commands.Customers;
using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Validations.Customers
{
    public abstract class CustomerValidator<TCommand, TResult> : CustomerValidator<TCommand>
        where TCommand : CustomerCommand<ICommandResult<TResult>>
    {
    }
}
