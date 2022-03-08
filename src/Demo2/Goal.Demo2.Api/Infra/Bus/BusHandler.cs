using Goal.Application.Seedwork.Handlers;
using Goal.Domain.Seedwork.Commands;
using Goal.Domain.Seedwork.Events;
using MediatR;

namespace Goal.Demo2.Api.Infra.Bus
{
    public sealed class InMemoryBusHandler : IBusHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public InMemoryBusHandler(IEventStore eventStore, IMediator mediator)
        {
            _eventStore = eventStore;
            _mediator = mediator;
        }

        public Task<ICommandResult> SendCommand<TCommand>(TCommand command)
            where TCommand : ICommand<ICommandResult>
            => _mediator.Send(command);

        public Task<ICommandResult<TResult>> SendCommand<TCommand, TResult>(TCommand command)
            where TCommand : ICommand<ICommandResult<TResult>>
            => _mediator.Send(command);

        public Task RaiseEvent<TEvent>(TEvent @event) where TEvent : Event
        {
            if (!@event.MessageType.Equals("DomainNotification"))
            {
                _eventStore?.Save(@event);
            }

            return _mediator.Publish(@event);
        }
    }
}
