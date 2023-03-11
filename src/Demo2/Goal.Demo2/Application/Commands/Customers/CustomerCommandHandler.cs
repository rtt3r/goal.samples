using FluentValidation.Results;
using Goal.Demo2.Application.Commands.Customers.Validators;
using Goal.Demo2.Application.Events.Customers;
using Goal.Demo2.Domain.Customers.Aggregates;
using Goal.Demo2.Infra.Crosscutting.Constants;
using Goal.Demo2.Infra.Data;
using Goal.Demo2.Model.Customers;
using Goal.Seedwork.Application.Commands;
using Goal.Seedwork.Application.Extensions;
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
        private readonly IDemo2UnitOfWork uow;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IDefaultNotificationHandler notificationHandler;
        private readonly ITypeAdapter typeAdapter;

        public CustomerCommandHandler(
            IDemo2UnitOfWork uow,
            IPublishEndpoint publishEndpoint,
            IDefaultNotificationHandler notificationHandler,
            ITypeAdapter typeAdapter)
        {
            this.uow = uow;
            this.publishEndpoint = publishEndpoint;
            this.notificationHandler = notificationHandler;
            this.typeAdapter = typeAdapter;
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

            if (await uow.Customers.GetByEmail(customer.Email) != null)
            {
                await notificationHandler.HandleAsync(
                    Notification.DomainViolation(
                        nameof(ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED),
                        ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED
                    ),
                    cancellationToken);

                return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
            }

            await uow.Customers.AddAsync(customer, cancellationToken);

            if (await SaveChangesAsync(cancellationToken))
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

            Customer customer = await uow.Customers.LoadAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                await notificationHandler.HandleAsync(
                    Notification.ResourceNotFound(
                        nameof(ApplicationConstants.Messages.CUSTOMER_NOT_FOUND),
                        ApplicationConstants.Messages.CUSTOMER_NOT_FOUND),
                    cancellationToken);

                return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
            }

            Customer existingCustomer = await uow.Customers.GetByEmail(customer.Email);

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

            uow.Customers.Update(existingCustomer);

            if (await SaveChangesAsync(cancellationToken))
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

            Customer customer = await uow.Customers.LoadAsync(command.CustomerId, cancellationToken);

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

            uow.Customers.Remove(customer);

            if (await SaveChangesAsync(cancellationToken))
            {
                await publishEndpoint.Publish(
                    new CustomerRemovedEvent(command.CustomerId),
                    cancellationToken);

                return CommandResult.Success();
            }

            return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
        }

        private async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (await uow.SaveAsync(cancellationToken))
            {
                return true;
            }

            await notificationHandler.HandleAsync(
                Notification.InternalError(
                    nameof(ApplicationConstants.Messages.SAVING_DATA_FAILURE),
                    ApplicationConstants.Messages.SAVING_DATA_FAILURE),
                cancellationToken);

            return false;
        }
    }
}
