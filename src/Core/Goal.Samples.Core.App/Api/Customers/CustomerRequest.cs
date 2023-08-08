namespace Goal.Samples.Core.App.Api.Customers;

public abstract class CustomerRequest
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
}
