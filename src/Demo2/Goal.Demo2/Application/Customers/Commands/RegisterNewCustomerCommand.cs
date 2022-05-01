using Goal.Demo2.Model.Customers;
using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Customers.Commands
{
    public class RegisterNewCustomerCommand : CustomerCommand<ICommandResult<CustomerModel>>
    {
        public string Email { get; set; }

        public RegisterNewCustomerCommand(string name, string email, DateTime birthDate)
        {
            Name = name;
            Email = email;
            BirthDate = birthDate;
        }
    }
}
