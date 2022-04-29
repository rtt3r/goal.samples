namespace Goal.Demo2.Api.Customers
{
    public abstract class CustomerRequest
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
