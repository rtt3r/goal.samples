namespace Goal.Samples.CQRS.App.Api.Customers;

public class RegisterNewCustomerRequest : CustomerRequest
{
    public string Email { get; set; }
}
