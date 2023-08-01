using Goal.Samples.CQRS.Model.Customers;
using Goal.Seedwork.Application.Commands;

namespace Goal.Samples.CQRS.Application.Commands.Customers;

public class RegisterNewCustomerCommand : CustomerCommand<ICommandResult<Customer>>
{
    public string Email { get; set; }

    public RegisterNewCustomerCommand(string name, string email, DateTime birthdate)
    {
        Name = name;
        Email = email;
        Birthdate = birthdate;
    }
}
