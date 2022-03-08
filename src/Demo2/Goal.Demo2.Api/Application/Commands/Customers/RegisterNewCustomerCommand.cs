using Goal.Demo2.Dto.Customers;
using Goal.Domain.Seedwork.Commands;

namespace Goal.Demo2.Api.Application.Commands.Customers
{
    public class RegisterNewCustomerCommand : CustomerCommand<ICommandResult<CustomerDto>>
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
