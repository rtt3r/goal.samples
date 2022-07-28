using Goal.Seedwork.Domain.Commands;

namespace Goal.Demo2.Application.Commands.Customers
{
    public abstract class CustomerCommand : Command<ICommandResult>
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
