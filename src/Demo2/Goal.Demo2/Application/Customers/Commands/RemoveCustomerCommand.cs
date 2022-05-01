using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Customers.Commands
{
    public class RemoveCustomerCommand : CustomerCommand<ICommandResult>
    {
        public string CustomerId { get; set; }

        public RemoveCustomerCommand(string customerId)
        {
            CustomerId = customerId;
        }
    }
}
