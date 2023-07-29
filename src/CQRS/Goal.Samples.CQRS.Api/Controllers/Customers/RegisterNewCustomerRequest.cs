namespace Goal.Samples.CQRS.Api.Controllers.Customers;

public class RegisterNewCustomerRequest : CustomerRequest
{
    public string Email { get; set; }
}
