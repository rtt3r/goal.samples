using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Commands.Customers
{
    public class UpdateCustomerCommand : CustomerCommand<ICommandResult>
    {
        public UpdateCustomerCommand(Guid id, string name, DateTime birthDate)
        {
            AggregateId = id;
            Name = name;
            BirthDate = birthDate;
        }
    }
}
