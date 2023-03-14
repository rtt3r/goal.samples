using Goal.Seedwork.Application.Commands;

namespace Goal.Samples.CQRS.Api.Application.Commands.Customers
{
    public class UpdateCustomerCommand : CustomerCommand<ICommandResult>
    {
        public string CustomerId { get; set; }

        public UpdateCustomerCommand(string customerId, string name, DateTime birthdate)
        {
            CustomerId = customerId;
            Name = name;
            Birthdate = birthdate;
        }
    }
}
