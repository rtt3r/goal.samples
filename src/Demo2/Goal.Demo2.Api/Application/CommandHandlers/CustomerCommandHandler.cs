using System.Text.Json;
using FluentValidation.Results;
using Goal.Application.Seedwork.Extensions;
using Goal.Application.Seedwork.Handlers;
using Goal.Demo2.Api.Application.Commands.Customers;
using Goal.Demo2.Api.Application.Events;
using Goal.Demo2.Api.Application.Validations.Customers;
using Goal.Demo2.Domain.Aggregates.Customers;
using Goal.Demo2.Model.Customers;
using Goal.Domain.Seedwork;
using Goal.Domain.Seedwork.Commands;
using Goal.Domain.Seedwork.Notifications;
using Goal.Infra.Crosscutting.Adapters;
using MediatR;

namespace Goal.Demo2.Api.Application.CommandHandlers
{
    public class CustomerCommandHandler : BaseCommandHandler,
        IRequestHandler<RegisterNewCustomerCommand, ICommandResult<CustomerModel>>,
        IRequestHandler<UpdateCustomerCommand, ICommandResult>,
        IRequestHandler<RemoveCustomerCommand, ICommandResult>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ITypeAdapter typeAdapter;
        private readonly ILogger<CustomerCommandHandler> logger;

        public CustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IBusHandler busHandler,
            INotificationHandler notificationHandler,
            ITypeAdapter typeAdapter,
            ILogger<CustomerCommandHandler> logger)
            : base(unitOfWork, busHandler, notificationHandler)
        {
            this.customerRepository = customerRepository;
            this.typeAdapter = typeAdapter;
            this.logger = logger;
        }

        public async Task<ICommandResult<CustomerModel>> Handle(RegisterNewCustomerCommand command, CancellationToken cancellationToken)
        {
            logger.LogTrace("Handle command {CommandName} started", nameof(RegisterNewCustomerCommand));
            logger.LogTrace("Validating command {Command}", JsonSerializer.Serialize(command));

            ValidationResult validationResult = await ValidateCommandAsync(
                new RegisterNewCustomerCommandValidation(),
                command,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                logger.LogTrace("Validation fail: {ValidationResult}", JsonSerializer.Serialize(validationResult));

                await NotifyViolations(validationResult, cancellationToken);
                return CommandResult.ValidationError<CustomerModel>(default);
            }

            logger.LogTrace($"Validation succeeded");

            var customer = new Customer(command.Name, command.Email, command.BirthDate);

            logger.LogTrace($"Checking customer email");

            if (await customerRepository.GetByEmail(customer.Email) != null)
            {
                await notificationHandler.Handle(
                    new Notification(command.MessageType, "The customer e-mail has already been taken."),
                    cancellationToken);

                logger.LogTrace("Email check fail: {Error}", "The customer e-mail has already been taken");

                return CommandResult.DomainError<CustomerModel>(default);
            }

            logger.LogTrace($"Email check succeeded");
            logger.LogTrace($"Saving new customer to database");

            await customerRepository.AddAsync(customer, cancellationToken);

            logger.LogTrace($"Commit data");

            if (await Commit(cancellationToken))
            {
                await busHandler.RaiseEvent(new CustomerRegisteredEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));

                return CommandResult.Success(
                    typeAdapter.ProjectAs<CustomerModel>(customer));
            }

            logger.LogTrace($"Commit failed");
            return CommandResult.DomainError<CustomerModel>(default);
        }

        public async Task<ICommandResult> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await ValidateCommandAsync(
                new UpdateCustomerCommandValidation(),
                command,
                cancellationToken);

            if (!validationResult.IsValid)
            {
                await NotifyViolations(validationResult, cancellationToken);
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
                await NotifyViolations(validationResult, cancellationToken);
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
