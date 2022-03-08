namespace Goal.Demo.Application.DTO.People.Requests
{
    public abstract class PersonRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cpf { get; set; }
    }
}
