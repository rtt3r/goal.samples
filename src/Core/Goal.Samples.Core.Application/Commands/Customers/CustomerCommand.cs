using Goal.Seedwork.Application.Commands;

namespace Goal.Samples.Core.Application.Commands.Customers;

public abstract class CustomerCommand : Command<ICommandResult>
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
}
