namespace Goal.Samples.CQRS.Api.Controllers.Customers
{
    public abstract class CustomerRequest
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
