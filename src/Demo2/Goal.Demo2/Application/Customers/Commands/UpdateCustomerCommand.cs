using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Customers.Commands
{
    public class UpdateCustomerCommand : CustomerCommand<ICommandResult>
    {
        public string CustomerId { get; set; }

        public UpdateCustomerCommand(string customerId, string name, DateTime birthDate)
        {
            CustomerId = customerId;
            Name = name;
            BirthDate = birthDate;
        }
    }
}
