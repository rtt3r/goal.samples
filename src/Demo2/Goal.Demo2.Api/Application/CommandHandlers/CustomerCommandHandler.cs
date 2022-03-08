using FluentValidation.Results;
using Goal.Application.Seedwork.Extensions;
using Goal.Application.Seedwork.Handlers;
using Goal.Demo2.Api.Application.Commands.Customers;
using Goal.Demo2.Api.Application.Events;
using Goal.Demo2.Api.Application.Validations.Customers;
using Goal.Demo2.Domain.Aggregates.Customers;
using Goal.Demo2.Dto.Customers;
using Goal.Domain.Seedwork;
using Goal.Domain.Seedwork.Commands;
using Goal.Domain.Seedwork.Notifications;
using Goal.Infra.Crosscutting.Adapters;
using MediatR;

namespace Goal.Demo2.Api.Application.CommandHandlers
{
    public class CustomerCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewCustomerCommand, ICommandResult<CustomerDto>>,
        IRequestHandler<UpdateCustomerCommand, ICommandResult>,
        IRequestHandler<RemoveCustomerCommand, ICommandResult>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ITypeAdapter typeAdapter;

        public CustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IBusHandler busHandler,
            INotificationHandler notificationHandler,
            ITypeAdapter typeAdapter)
            : base(unitOfWork, busHandler, notificationHandler)
        {
            this.customerRepository = customerRepository;
            this.typeAdapter = typeAdapter;
        }

        public async Task<ICommandResult<CustomerDto>> Handle(RegisterNewCustomerCommand command, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await ValidateCommandAsync(
                new RegisterNewCustomerCommandValidation(),
                command,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                await NotifyValidationErrors(validationResult, cancellationToken);
                return CommandResult.ValidationError<CustomerDto>(default);
            }

            var customer = new Customer(command.Name, command.Email, command.BirthDate);

            if (await customerRepository.GetByEmail(customer.Email) != null)
            {
                await notificationHandler.Handle(
                    new Notification(command.MessageType, "The customer e-mail has already been taken."),
                    cancellationToken);

                return CommandResult.DomainError<CustomerDto>(default);
            }

            await customerRepository.AddAsync(customer, cancellationToken);

            if (await Commit(cancellationToken))
            {
                await busHandler.RaiseEvent(new CustomerRegisteredEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));

                return CommandResult.Success(
                    typeAdapter.ProjectAs<CustomerDto>(customer));
            }

            return CommandResult.DomainError<CustomerDto>(default);
        }

        public async Task<ICommandResult> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await ValidateCommandAsync(
                new UpdateCustomerCommandValidation(),
                command,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                await NotifyValidationErrors(validationResult, cancellationToken);
                return CommandResult.ValidationError();
            }

            Customer customer = await customerRepository.LoadAsync(command.AggregateId, cancellationToken);

            if (customer is null)
            {
                await notificationHandler.Handle(
                    new Notification(command.MessageType, "The customer was not found."),
                    cancellationToken);

                return CommandResult.DomainError();
            }

            Customer existingCustomer = await customerRepository.GetByEmail(customer.Email);

            if (existingCustomer != null && !existingCustomer.Equals(customer))
            {
                await notificationHandler.Handle(
                    new Notification(command.MessageType, "The customer e-mail has already been taken."),
                    cancellationToken);

                return CommandResult.DomainError();
            }

            customer.UpdateName(command.Name);
            customer.UpdateBirthDate(command.BirthDate);

            customerRepository.Update(existingCustomer);

            if (await Commit(cancellationToken))
            {
                await busHandler.RaiseEvent(new CustomerUpdatedEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));
                return CommandResult.Success();
            }

            return CommandResult.DomainError();
        }

        public async Task<ICommandResult> Handle(RemoveCustomerCommand command, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await ValidateCommandAsync(
                new RemoveCustomerCommandValidation(),
                command,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                await NotifyValidationErrors(validationResult, cancellationToken);
                return CommandResult.ValidationError();
            }

            Customer customer = await customerRepository.LoadAsync(command.AggregateId, cancellationToken);

            if (customer is null)
            {
                await notificationHandler.Handle(
                    new Notification(command.MessageType, "The customer was not found."),
                    cancellationToken);

                return CommandResult.DomainError();
            }

            customerRepository.Remove(customer);

            if (await Commit(cancellationToken))
            {
                await busHandler.RaiseEvent(new CustomerRemovedEvent(command.AggregateId));
                return CommandResult.Success();
            }

            return CommandResult.DomainError();
        }
    }
}
