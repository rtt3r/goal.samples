using FluentValidation.Results;
using Goal.Demo2.Application.Commands.Customers;
using Goal.Demo2.Application.Commands.Customers.Validators;
using Goal.Demo2.Domain.Customers.Aggregates;
using Goal.Demo2.Domain.Customers.Events;
using Goal.Demo2.Infra.Crosscutting.Constants;
using Goal.Demo2.Model.Customers;
using Goal.Seedwork.Application.Extensions;
using Goal.Seedwork.Domain;
using Goal.Seedwork.Domain.Commands;
using Goal.Seedwork.Infra.Crosscutting.Adapters;
using Goal.Seedwork.Infra.Crosscutting.Notifications;
using MassTransit;

namespace Goal.Demo2.Application.Handlers.Customers
{
    public class CustomerCommandHandler :
        ICommandHandler<RegisterNewCustomerCommand, ICommandResult<CustomerModel>>,
        ICommandHandler<UpdateCustomerCommand, ICommandResult>,
        ICommandHandler<RemoveCustomerCommand, ICommandResult>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly INotificationHandler notificationHandler;
        private readonly ITypeAdapter typeAdapter;
        private readonly ILogger<CustomerCommandHandler> logger;

        public CustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IPublishEndpoint publishEndpoint,
            INotificationHandler notificationHandler,
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
                    await notificationHandler.AddNotificationAsync(
                        Notification.InputValidation(
                            error.ErrorCode,
                            error.ErrorMessage,
                            error.PropertyName),
                        cancellationToken);
                }

                return CommandResult.Fail<CustomerModel>(default);
            }

            var customer = new Customer(command.Name, command.Email, command.Birthdate);

            if (await customerRepository.GetByEmail(customer.Email) != null)
            {
                await notificationHandler.AddNotificationAsync(
                    Notification.DomainViolation(
                        ApplicationConstants.ErrorCodes.CustomerEmailDuplicated,
                        ApplicationConstants.Messages.CustomerEmailDuplicated
                    ),
                    cancellationToken);

                return CommandResult.Fail<CustomerModel>(default);
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

            return CommandResult.Fail<CustomerModel>(default);
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
                    await notificationHandler.AddNotificationAsync(
                        Notification.InputValidation(
                            error.ErrorCode,
                            error.ErrorMessage,
                            error.PropertyName),
                        cancellationToken);
                }

                return CommandResult.Fail<CustomerModel>(default);
            }

            Customer customer = await customerRepository.LoadAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                await notificationHandler.AddNotificationAsync(
                    Notification.DomainViolation(
                        ApplicationConstants.ErrorCodes.CustomerNotFound,
                        ApplicationConstants.Messages.CustomerNotFound),
                    cancellationToken);

                return CommandResult.Fail<CustomerModel>(default);
            }

            Customer existingCustomer = await customerRepository.GetByEmail(customer.Email);

            if (existingCustomer != null && existingCustomer != customer)
            {
                await notificationHandler.AddNotificationAsync(
                    Notification.DomainViolation(
                        ApplicationConstants.ErrorCodes.CustomerEmailDuplicated,
                        ApplicationConstants.Messages.CustomerEmailDuplicated
                    ),
                    cancellationToken);

                return CommandResult.Fail<CustomerModel>(default);
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

            return CommandResult.Fail<CustomerModel>(default);
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
                    await notificationHandler.AddNotificationAsync(
                        Notification.InputValidation(
                            error.ErrorCode,
                            error.ErrorMessage,
                            error.PropertyName),
                        cancellationToken);
                }

                return CommandResult.Fail<CustomerModel>(default);
            }

            Customer customer = await customerRepository.LoadAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                await notificationHandler.AddNotificationAsync(
                    Notification.DomainViolation(
                        ApplicationConstants.ErrorCodes.CustomerNotFound,
                        ApplicationConstants.Messages.CustomerNotFound
                    ),
                    cancellationToken);

                return CommandResult.Fail<CustomerModel>(default);
            }

            customerRepository.Remove(customer);

            if (await Commit(cancellationToken))
            {
                await publishEndpoint.Publish(
                    new CustomerRemovedEvent(command.CustomerId),
                    cancellationToken);

                return CommandResult.Success();
            }

            return CommandResult.Fail<CustomerModel>(default);
        }

        private async Task<bool> Commit(CancellationToken cancellationToken = new CancellationToken())
        {
            if (unitOfWork.Commit())
            {
                return true;
            }

            await notificationHandler.AddNotificationAsync(
                Notification.InternalError(
                    ApplicationConstants.ErrorCodes.SaveDataFailed,
                    ApplicationConstants.Messages.SaveDataFailed
                ),
                cancellationToken);

            return false;
        }
    }
}
