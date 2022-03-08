using Goal.Demo2.Api.Application.Commands.Customers;
using Goal.Domain.Seedwork.Commands;

namespace Goal.Demo2.Api.Application.Validations.Customers
{
    public abstract class CustomerValidation<TCommand, TResult> : CustomerValidation<TCommand>
        where TCommand : CustomerCommand<ICommandResult<TResult>>
    {
    }
}
