using Goal.Seedwork.Application.Commands;

namespace Goal.Samples.CQRS.Application.Commands.Customers.Validators
{
    public abstract class CustomerValidator<TCommand, TResult> : CustomerValidator<TCommand>
        where TCommand : CustomerCommand<ICommandResult<TResult>>
    {
    }
}
