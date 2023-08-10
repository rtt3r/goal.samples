using Goal.Seedwork.Application.Commands;

namespace Goal.Samples.Core.Application.Commands.Customers;

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
