namespace Goal.Samples.Core.Api.Controllers.Customers;

public abstract class CustomerRequest
{
    public string Name { get; set; }
    public DateTime Birthdate { get; set; }
}
