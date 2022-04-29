using Goal.Seedwork.Application.Handlers;
using Goal.Seedwork.Domain.Commands;
using Goal.Seedwork.Domain.Events;
using MediatR;

namespace Goal.Demo2.Application.Bus
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

        public Task RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _eventStore?.Save(@event);
            return _mediator.Publish(@event);
        }
    }
}
