using FluentValidation.Results;
using Goal.Samples.CQRS.Application.Commands.Customers.Validators;
using Goal.Samples.CQRS.Application.Events.Customers;
using Goal.Samples.CQRS.Domain.Customers.Aggregates;
using Goal.Samples.CQRS.Infra.Data;
using Goal.Samples.CQRS.Model.Customers;
using Goal.Samples.Infra.Crosscutting;
using Goal.Samples.Infra.Crosscutting.Constants;
using Goal.Seedwork.Application.Commands;
using Goal.Seedwork.Application.Extensions;
using Goal.Seedwork.Infra.Crosscutting.Adapters;
using Goal.Seedwork.Infra.Crosscutting.Notifications;
using MassTransit;

namespace Goal.Samples.CQRS.Application.Commands.Customers;

public class CustomerCommandHandler :
    ICommandHandler<RegisterNewCustomerCommand, ICommandResult<Model.Customers.Customer>>,
    ICommandHandler<UpdateCustomerCommand, ICommandResult>,
    ICommandHandler<RemoveCustomerCommand, ICommandResult>
{
    private readonly ICqrsUnitOfWork uow;
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IDefaultNotificationHandler notificationHandler;
    private readonly ITypeAdapter typeAdapter;
    private readonly AppState appState;

    public CustomerCommandHandler(
        ICqrsUnitOfWork uow,
        IPublishEndpoint publishEndpoint,
        IDefaultNotificationHandler notificationHandler,
        ITypeAdapter typeAdapter,
        AppState appState)
    {
        this.uow = uow;
        this.publishEndpoint = publishEndpoint;
        this.notificationHandler = notificationHandler;
        this.typeAdapter = typeAdapter;
        this.appState = appState;
    }

    public async Task<ICommandResult<Model.Customers.Customer>> Handle(RegisterNewCustomerCommand command, CancellationToken cancellationToken)
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

            return CommandResult.Failure<Model.Customers.Customer>(default, notificationHandler.GetNotifications());
        }

        Domain.Customers.Aggregates.Customer customer = await uow.Customers.GetByEmail(command.Email);

        if (customer != null)
        {
            await notificationHandler.HandleAsync(
                Notification.DomainViolation(
                    nameof(ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED),
                    ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED
                ),
                cancellationToken);

            return CommandResult.Failure<Model.Customers.Customer>(default, notificationHandler.GetNotifications());
        }

        customer = new Domain.Customers.Aggregates.Customer(command.Name, command.Email, command.Birthdate);

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
                typeAdapter.ProjectAs<Model.Customers.Customer>(customer));
        }

        return CommandResult.Failure<Model.Customers.Customer>(default, notificationHandler.GetNotifications());
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

            return CommandResult.Failure<Model.Customers.Customer>(default, notificationHandler.GetNotifications());
        }

        Domain.Customers.Aggregates.Customer customer = await uow.Customers.LoadAsync(command.CustomerId, cancellationToken);

        if (customer is null)
        {
            await notificationHandler.HandleAsync(
                Notification.ResourceNotFound(
                    nameof(ApplicationConstants.Messages.CUSTOMER_NOT_FOUND),
                    ApplicationConstants.Messages.CUSTOMER_NOT_FOUND),
                cancellationToken);

            return CommandResult.Failure<Model.Customers.Customer>(default, notificationHandler.GetNotifications());
        }

        Domain.Customers.Aggregates.Customer existingCustomer = await uow.Customers.GetByEmail(customer.Email);

        if (existingCustomer != null && existingCustomer != customer)
        {
            await notificationHandler.HandleAsync(
                Notification.DomainViolation(
                    nameof(ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED),
                    ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED
                ),
                cancellationToken);

            return CommandResult.Failure<Model.Customers.Customer>(default, notificationHandler.GetNotifications());
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

        return CommandResult.Failure<Model.Customers.Customer>(default, notificationHandler.GetNotifications());
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

            return CommandResult.Failure<Model.Customers.Customer>(default, notificationHandler.GetNotifications());
        }

        Domain.Customers.Aggregates.Customer customer = await uow.Customers.LoadAsync(command.CustomerId, cancellationToken);

        if (customer is null)
        {
            await notificationHandler.HandleAsync(
                Notification.DomainViolation(
                    nameof(ApplicationConstants.Messages.CUSTOMER_NOT_FOUND),
                    ApplicationConstants.Messages.CUSTOMER_NOT_FOUND
                ),
                cancellationToken);

            return CommandResult.Failure<Model.Customers.Customer>(default, notificationHandler.GetNotifications());
        }

        uow.Customers.Remove(customer);

        if (await SaveChangesAsync(cancellationToken))
        {
            await publishEndpoint.Publish(
                new CustomerRemovedEvent(command.CustomerId),
                cancellationToken);

            return CommandResult.Success();
        }

        return CommandResult.Failure<Model.Customers.Customer>(default, notificationHandler.GetNotifications());
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
