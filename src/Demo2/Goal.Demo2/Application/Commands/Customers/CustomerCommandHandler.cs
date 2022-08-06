using FluentValidation.Results;
using Goal.Demo2.Application.Commands.Customers.Validators;
using Goal.Demo2.Application.Events.Customers;
using Goal.Demo2.Domain.Customers.Aggregates;
using Goal.Demo2.Infra.Crosscutting.Constants;
using Goal.Demo2.Model.Customers;
using Goal.Seedwork.Application.Commands;
using Goal.Seedwork.Application.Extensions;
using Goal.Seedwork.Domain;
using Goal.Seedwork.Infra.Crosscutting.Adapters;
using Goal.Seedwork.Infra.Crosscutting.Notifications;
using MassTransit;

namespace Goal.Demo2.Application.Commands.Customers
{
    public class CustomerCommandHandler :
        ICommandHandler<RegisterNewCustomerCommand, ICommandResult<CustomerModel>>,
        ICommandHandler<UpdateCustomerCommand, ICommandResult>,
        ICommandHandler<RemoveCustomerCommand, ICommandResult>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IDefaultNotificationHandler notificationHandler;
        private readonly ITypeAdapter typeAdapter;
        private readonly ILogger<CustomerCommandHandler> logger;

        public CustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IPublishEndpoint publishEndpoint,
            IDefaultNotificationHandler notificationHandler,
            ITypeAdapter typeAdapter,
            ILogger<CustomerCommandHandler> logger)
        {
            this.customerRepository = customerRepository;
            this.unitOfWork = unitOfWork;
            this.publishEndpoint = publishEndpoint;
            this.notificationHandler = notificationHandler;
            this.typeAdapter = typeAdapter;
            this.logger = logger;
        }

        public async Task<ICommandResult<CustomerModel>> Handle(RegisterNewCustomerCommand command, CancellationToken cancellationToken)
        {
            FluentValidation.Results.ValidationResult validationResult = await command.ValidateCommandAsync(
                new RegisterNewCustomerCommandValidator(),
                cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure error in validationResult.Errors)
                {
                    await notificationHandler.HandleAsync(
                        Notification.InputValidation(
                            error.ErrorCode,
                            error.ErrorMessage,
                            error.PropertyName),
                        cancellationToken);
                }

                return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
            }

            var customer = new Customer(command.Name, command.Email, command.Birthdate);

            if (await customerRepository.GetByEmail(customer.Email) != null)
            {
                await notificationHandler.HandleAsync(
                    Notification.DomainViolation(
                        nameof(ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED),
                        ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED
                    ),
                    cancellationToken);

                return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
            }

            await customerRepository.AddAsync(customer, cancellationToken);

            if (await Commit(cancellationToken))
            {
                await publishEndpoint.Publish(
                    new CustomerRegisteredEvent(
                        customer.Id,
                        customer.Name,
                        customer.Email,
                        customer.Birthdate),
                    cancellationToken);

                return CommandResult.Success(
                    typeAdapter.ProjectAs<CustomerModel>(customer));
            }

            return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
        }

        public async Task<ICommandResult> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            FluentValidation.Results.ValidationResult validationResult = await command.ValidateCommandAsync(
                new UpdateCustomerCommandValidator(),
                cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure error in validationResult.Errors)
                {
                    await notificationHandler.HandleAsync(
                        Notification.InputValidation(
                            error.ErrorCode,
                            error.ErrorMessage,
                            error.PropertyName),
                        cancellationToken);
                }

                return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
            }

            Customer customer = await customerRepository.LoadAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                await notificationHandler.HandleAsync(
                    Notification.ResourceNotFound(
                        nameof(ApplicationConstants.Messages.CUSTOMER_NOT_FOUND),
                        ApplicationConstants.Messages.CUSTOMER_NOT_FOUND),
                    cancellationToken);

                return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
            }

            Customer existingCustomer = await customerRepository.GetByEmail(customer.Email);

            if (existingCustomer != null && existingCustomer != customer)
            {
                await notificationHandler.HandleAsync(
                    Notification.DomainViolation(
                        nameof(ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED),
                        ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED
                    ),
                    cancellationToken);

                return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
            }

            customer.UpdateName(command.Name);
            customer.UpdateBirthdate(command.Birthdate);

            customerRepository.Update(existingCustomer);

            if (await Commit(cancellationToken))
            {
                await publishEndpoint.Publish(
                    new CustomerUpdatedEvent(
                        customer.Id,
                        customer.Name,
                        customer.Email,
                        customer.Birthdate),
                    cancellationToken);

                return CommandResult.Success();
            }

            return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
        }

        public async Task<ICommandResult> Handle(RemoveCustomerCommand command, CancellationToken cancellationToken)
        {
            FluentValidation.Results.ValidationResult validationResult = await command.ValidateCommandAsync(
                new RemoveCustomerCommandValidator(),
                cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure error in validationResult.Errors)
                {
                    await notificationHandler.HandleAsync(
                        Notification.InputValidation(
                            error.ErrorCode,
                            error.ErrorMessage,
                            error.PropertyName),
                        cancellationToken);
                }

                return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
            }

            Customer customer = await customerRepository.LoadAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                await notificationHandler.HandleAsync(
                    Notification.DomainViolation(
                        nameof(ApplicationConstants.Messages.CUSTOMER_NOT_FOUND),
                        ApplicationConstants.Messages.CUSTOMER_NOT_FOUND
                    ),
                    cancellationToken);

                return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
            }

            customerRepository.Remove(customer);

            if (await Commit(cancellationToken))
            {
                await publishEndpoint.Publish(
                    new CustomerRemovedEvent(command.CustomerId),
                    cancellationToken);

                return CommandResult.Success();
            }

            return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
        }

        private async Task<bool> Commit(CancellationToken cancellationToken = new CancellationToken())
        {
            if (unitOfWork.Commit())
            {
                return true;
            }

            await notificationHandler.HandleAsync(
                Notification.InternalError(
                    nameof(ApplicationConstants.Messages.SAVING_DATA_FAILURE),
                    ApplicationConstants.Messages.SAVING_DATA_FAILURE
                ),
                cancellationToken);

            return false;
        }
    }
}
