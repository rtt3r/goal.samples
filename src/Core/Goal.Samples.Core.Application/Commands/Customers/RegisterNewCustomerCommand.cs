using Goal.Samples.Core.Model.Customers;
using Goal.Seedwork.Application.Commands;

namespace Goal.Samples.Core.Application.Commands.Customers;

public class RegisterNewCustomerCommand : CustomerCommand<ICommandResult<Customer>>
{
    public string Email { get; set; }

    public RegisterNewCustomerCommand(string name, string email, DateTime birthDate)
    {
        Name = name;
        Email = email;
        Birthdate = birthDate;
    }
}
