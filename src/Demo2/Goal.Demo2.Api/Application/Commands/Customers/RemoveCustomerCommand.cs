using Goal.Domain.Seedwork.Commands;

namespace Goal.Demo2.Api.Application.Commands.Customers
{
    public class RemoveCustomerCommand : CustomerCommand<ICommandResult>
    {
        public RemoveCustomerCommand(Guid id)
        {
            AggregateId = id;
        }
    }
}
