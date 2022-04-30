using Goal.Demo2.Application.Events;
using Goal.Demo2.Infra.Data.Query.Repositories.Customers;
using Goal.Demo2.Model.Customers;
using MediatR;

namespace Goal.Demo2.Application.EventHandlers
{
    public class CustomerEventHandler :
        INotificationHandler<CustomerRegisteredEvent>,
        INotificationHandler<CustomerUpdatedEvent>,
        INotificationHandler<CustomerRemovedEvent>
    {
        private readonly ICustomerQueryRepository customerRepository;

        public CustomerEventHandler(ICustomerQueryRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task Handle(CustomerUpdatedEvent message, CancellationToken cancellationToken)
        {
            CustomerModel customer = await customerRepository.LoadAsync(message.AggregateId.ToString(), cancellationToken);

            customer.CustomerId = message.AggregateId.ToString();
            customer.Name = message.Name;
            customer.BirthDate = message.BirthDate;
            customer.Email = message.Email;

            await customerRepository.StoreAsync(
                message.AggregateId.ToString(),
                customer,
                cancellationToken);
        }

        public async Task Handle(CustomerRegisteredEvent message, CancellationToken cancellationToken)
        {
            await customerRepository.StoreAsync(
                message.AggregateId.ToString(),
                new CustomerModel
                {
                    CustomerId = message.AggregateId.ToString(),
                    Name = message.Name,
                    BirthDate = message.BirthDate,
                    Email = message.Email,
                },
                cancellationToken);
        }

        public async Task Handle(CustomerRemovedEvent message, CancellationToken cancellationToken)
            => await customerRepository.RemoveAsync(message.AggregateId.ToString(), cancellationToken);
    }
}