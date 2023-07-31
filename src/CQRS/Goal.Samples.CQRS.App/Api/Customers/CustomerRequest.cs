namespace Goal.Samples.CQRS.App.Api.Customers;

public abstract class CustomerRequest
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
}
