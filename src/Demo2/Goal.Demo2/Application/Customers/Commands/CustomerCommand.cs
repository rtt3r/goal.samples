using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Customers.Commands
{
    public abstract class CustomerCommand : Command<ICommandResult>
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
