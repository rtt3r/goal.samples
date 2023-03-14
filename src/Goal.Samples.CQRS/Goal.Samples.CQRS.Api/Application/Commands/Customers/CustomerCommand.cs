using Goal.Seedwork.Application.Commands;

namespace Goal.Samples.CQRS.Api.Application.Commands.Customers
{
    public abstract class CustomerCommand : Command<ICommandResult>
    {
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
