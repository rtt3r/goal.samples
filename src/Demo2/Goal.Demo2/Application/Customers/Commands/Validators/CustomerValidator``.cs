using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Customers.Commands.Validators
{
    public abstract class CustomerValidator<TCommand, TResult> : CustomerValidator<TCommand>
        where TCommand : CustomerCommand<ICommandResult<TResult>>
    {
    }
}
