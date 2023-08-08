using Goal.Samples.Core.Application.Events.Customers;
using Goal.Samples.Core.Infra.Data.Query.Repositories.Customers;
using Goal.Samples.Core.Model.Customers;
using Goal.Seedwork.Domain.Events;
using MassTransit;

namespace Goal.Samples.Core.Worker.Consumers.Customers;

public class CustomerUpdatedEventConsumer : EventConsumer<CustomerUpdatedEvent>
{
    private readonly ICustomerQueryRepository customerRepository;

    public CustomerUpdatedEventConsumer(
        ICustomerQueryRepository customerRepository,
        IEventStore eventStore,
        ILogger<CustomerUpdatedEventConsumer> logger)
        : base(eventStore, logger)
    {
        this.customerRepository = customerRepository;
    }

    protected override async Task HandleEvent(CustomerUpdatedEvent @event, CancellationToken cancellationToken = default)
    {
        Customer customer = await customerRepository.LoadAsync(@event.AggregateId, cancellationToken);

        customer.Name = @event.Name;
        customer.Birthdate = @event.Birthdate;
        customer.Email = @event.Email;

        await customerRepository.StoreAsync(
            @event.AggregateId,
            customer,
            cancellationToken);
    }

    public class ConsumerDefinition : ConsumerDefinition<CustomerUpdatedEventConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CustomerUpdatedEventConsumer> consumerConfigurator)
            => consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}
