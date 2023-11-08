using Goal.Samples.Core.Application.Commands.Customers.Validators;
using Goal.Samples.Core.Application.Events.Customers;
using Goal.Samples.Core.Domain.Customers.Aggregates;
using Goal.Samples.Core.Infra.Data;
using Goal.Samples.Infra.Crosscutting;
using Goal.Samples.Infra.Crosscutting.Constants;
using Goal.Seedwork.Application.Commands;
using Goal.Seedwork.Application.Extensions;
using Goal.Seedwork.Infra.Crosscutting.Adapters;
using Goal.Seedwork.Infra.Crosscutting.Notifications;
using MassTransit;
using CustomerModel = Goal.Samples.Core.Model.Customers.Customer;

namespace Goal.Samples.Core.Application.Commands.Customers;

public class CustomerCommandHandler : CommandHandlerBase,
    ICommandHandler<RegisterNewCustomerCommand, ICommandResult<CustomerModel>>,
    ICommandHandler<UpdateCustomerCommand, ICommandResult>,
    ICommandHandler<RemoveCustomerCommand, ICommandResult>
{
    public CustomerCommandHandler(
        ICoreUnitOfWork uow,
        IPublishEndpoint publishEndpoint,
        IDefaultNotificationHandler notificationHandler,
        ITypeAdapter typeAdapter,
        AppState appState)
        : base(uow, publishEndpoint, notificationHandler, typeAdapter, appState)
    {
    }

    public async Task<ICommandResult<CustomerModel>> Handle(RegisterNewCustomerCommand command, CancellationToken cancellationToken)
    {
        if (!await ValidateCommandAsync<RegisterNewCustomerCommandValidator, RegisterNewCustomerCommand>(command, cancellationToken))
        {
            return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
        }

        Customer customer = await uow.Customers.GetByEmail(command.Email);

        if (customer != null)
        {
            await HandleDomainViolationAsync(
                nameof(ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED),
                ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED,
                cancellationToken);

            return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
        }

        customer = new Customer(command.Name, command.Email, command.Birthdate);

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
        if (!await ValidateCommandAsync<UpdateCustomerCommandValidator, UpdateCustomerCommand>(command, cancellationToken))
        {
            return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
        }

        Customer customer = await uow.Customers.LoadAsync(command.CustomerId, cancellationToken);

        if (customer is null)
        {
            await HandleResourceNotFoundAsync(
                nameof(ApplicationConstants.Messages.CUSTOMER_NOT_FOUND),
                ApplicationConstants.Messages.CUSTOMER_NOT_FOUND,
                cancellationToken);

            return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
        }

        Customer existingCustomer = await uow.Customers.GetByEmail(customer.Email);

        if (existingCustomer != null && existingCustomer != customer)
        {
            await HandleDomainViolationAsync(
                nameof(ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED),
                ApplicationConstants.Messages.CUSTOMER_EMAIL_DUPLICATED,
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
        if (!await ValidateCommandAsync<RemoveCustomerCommandValidator, RemoveCustomerCommand>(command, cancellationToken))
        {
            return CommandResult.Failure<CustomerModel>(default, notificationHandler.GetNotifications());
        }

        Customer customer = await uow.Customers.LoadAsync(command.CustomerId, cancellationToken);

        if (customer is null)
        {
            await HandleResourceNotFoundAsync(
                nameof(ApplicationConstants.Messages.CUSTOMER_NOT_FOUND),
                ApplicationConstants.Messages.CUSTOMER_NOT_FOUND,
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
}
