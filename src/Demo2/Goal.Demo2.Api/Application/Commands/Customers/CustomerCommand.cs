using Goal.Domain.Seedwork.Commands;

namespace Goal.Demo2.Api.Application.Commands.Customers
{
    public abstract class CustomerCommand : Command<ICommandResult>
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
