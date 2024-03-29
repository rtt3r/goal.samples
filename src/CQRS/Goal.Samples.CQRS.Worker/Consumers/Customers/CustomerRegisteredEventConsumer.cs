using Goal.Samples.CQRS.Application.Events.Customers;
using Goal.Samples.CQRS.Infra.Data.Query.Repositories.Customers;
using Goal.Samples.CQRS.Model.Customers;
using Goal.Seedwork.Domain.Events;
using MassTransit;

namespace Goal.Samples.CQRS.Worker.Consumers.Customers
{
    public class CustomerRegisteredEventConsumer : EventConsumer<CustomerRegisteredEvent>
    {
        private readonly ICustomerQueryRepository customerRepository;

        public CustomerRegisteredEventConsumer(
            ICustomerQueryRepository customerRepository,
            IEventStore eventStore,
            ILogger<CustomerRegisteredEventConsumer> logger)
            : base(eventStore, logger)
        {
            this.customerRepository = customerRepository;
        }

        protected override async Task HandleEvent(CustomerRegisteredEvent @event, CancellationToken cancellationToken = default)
        {
            await customerRepository.StoreAsync(
                @event.AggregateId,
                new CustomerModel
                {
                    CustomerId = @event.AggregateId,
                    Name = @event.Name,
                    Birthdate = @event.Birthdate,
                    Email = @event.Email,
                },
                cancellationToken);
        }

        public class ConsumerDefinition : ConsumerDefinition<CustomerRegisteredEventConsumer>
        {
            protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CustomerRegisteredEventConsumer> consumerConfigurator) => consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
        }
    }
}
