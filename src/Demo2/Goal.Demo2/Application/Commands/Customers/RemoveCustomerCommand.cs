using Goal.Seedwork.Application.Commands;

namespace Goal.Demo2.Application.Commands.Customers
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
