namespace Goal.Demo2.Api.Application.Dtos.Customers.Requests
{
    public abstract class CustomerRequest
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
