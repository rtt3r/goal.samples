using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Commands.Customers
{
    public class RemoveCustomerCommand : CustomerCommand<ICommandResult>
    {
        public RemoveCustomerCommand(Guid id)
        {
            AggregateId = id;
        }
    }
}
