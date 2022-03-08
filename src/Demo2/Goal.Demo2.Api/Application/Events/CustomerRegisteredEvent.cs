using Goal.Domain.Seedwork.Events;

namespace Goal.Demo2.Api.Application.Events
{
    public class CustomerRegisteredEvent : Event
    {
        public CustomerRegisteredEvent(Guid id, string name, string email, DateTime birthDate)
        {
            AggregateId = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
        }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }
    }
}
