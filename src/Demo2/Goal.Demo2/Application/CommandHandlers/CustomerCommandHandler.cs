using FluentValidation.Results;
using Goal.Demo2.Application.Commands.Customers;
using Goal.Demo2.Application.Events;
using Goal.Demo2.Application.Validations.Customers;
using Goal.Demo2.Domain.Aggregates.Customers;
using Goal.Demo2.Infra.Crosscutting.Constants;
using Goal.Demo2.Model.Customers;
using Goal.Seedwork.Application.Extensions;
using Goal.Seedwork.Application.Handlers;
using Goal.Seedwork.Domain;
using Goal.Seedwork.Domain.Commands;
using Goal.Seedwork.Domain.Notifications;
using Goal.Seedwork.Infra.Crosscutting.Adapters;

namespace Goal.Demo2.Application.CommandHandlers
{
    public class CustomerCommandHandler :
        ICommandHandler<RegisterNewCustomerCommand, ICommandResult<CustomerModel>>,
        ICommandHandler<UpdateCustomerCommand, ICommandResult>,
        ICommandHandler<RemoveCustomerCommand, ICommandResult>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IBusHandler busHandler;
        private readonly INotificationHandler notificationHandler;
        private readonly ITypeAdapter typeAdapter;
        private readonly ILogger<CustomerCommandHandler> logger;

        public CustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IBusHandler busHandler,
            INotificationHandler notificationHandler,
            ITypeAdapter typeAdapter,
            ILogger<CustomerCommandHandler> logger)
        {
            this.customerRepository = customerRepository;
            this.unitOfWork = unitOfWork;
            this.busHandler = busHandler;
            this.notificationHandler = notificationHandler;
            this.typeAdapter = typeAdapter;
            this.logger = logger;
        }

        public async Task<ICommandResult<CustomerModel>> Handle(RegisterNewCustomerCommand command, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await command.ValidateCommandAsync(
                new RegisterNewCustomerCommandValidator(),
                cancellationToken);

            if (!validationResult.IsValid)
            {
                await NotifyViolations(validationResult, cancellationToken);
                return CommandResult.ValidationError<CustomerModel>(default);
            }

            var customer = new Customer(command.Name, command.Email, command.BirthDate);

            if (await customerRepository.GetByEmail(customer.Email) != null)
            {
                await notificationHandler.Handle(
                    Notification.Violation(
                        ApplicationConstants.ErrorCodes.CustomerEmailDuplicated,
                        ApplicationConstants.Messages.CustomerEmailDuplicated),
                    cancellationToken);

                return CommandResult.DomainError<CustomerModel>(default);
            }

            await customerRepository.AddAsync(customer, cancellationToken);

            if (await Commit(cancellationToken))
            {
                await busHandler.RaiseEvent(new CustomerRegisteredEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));

                return CommandResult.Success(
                    typeAdapter.ProjectAs<CustomerModel>(customer));
            }

            return CommandResult.DomainError<CustomerModel>(default);
        }

        public async Task<ICommandResult> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await command.ValidateCommandAsync(
                new UpdateCustomerCommandValidator(),
                cancellationToken);

            if (!validationResult.IsValid)
            {
                await NotifyViolations(validationResult, cancellationToken);
                return CommandResult.ValidationError();
            }

            Customer customer = await customerRepository.LoadAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                await notificationHandler.Handle(
                    Notification.Violation(
                        ApplicationConstants.ErrorCodes.CustomerNotFound,
                        ApplicationConstants.Messages.CustomerNotFound),
                    cancellationToken);

                return CommandResult.DomainError();
            }

            Customer existingCustomer = await customerRepository.GetByEmail(customer.Email);

            if (existingCustomer != null && !existingCustomer.Equals(customer))
            {
                await notificationHandler.Handle(
                    Notification.Violation(
                        ApplicationConstants.ErrorCodes.CustomerEmailDuplicated,
                        ApplicationConstants.Messages.CustomerEmailDuplicated),
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
            ValidationResult validationResult = await command.ValidateCommandAsync(
                new RemoveCustomerCommandValidator(),
                cancellationToken);

            if (!validationResult.IsValid)
            {
                await NotifyViolations(validationResult, cancellationToken);
                return CommandResult.ValidationError();
            }

            Customer customer = await customerRepository.LoadAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                await notificationHandler.Handle(
                    Notification.Violation(
                        ApplicationConstants.ErrorCodes.CustomerNotFound,
                        ApplicationConstants.Messages.CustomerNotFound),
                    cancellationToken);

                return CommandResult.DomainError();
            }

            customerRepository.Remove(customer);

            if (await Commit(cancellationToken))
            {
                await busHandler.RaiseEvent(new CustomerRemovedEvent(command.CustomerId));
                return CommandResult.Success();
            }

            return CommandResult.DomainError();
        }

        private async Task NotifyViolations(
            ValidationResult validationResult,
            CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (ValidationFailure error in validationResult.Errors)
            {
                await notificationHandler.Handle(
                    Notification.Violation(error.ErrorCode, error.ErrorMessage, error.PropertyName),
                    cancellationToken);
            }
        }

        private async Task<bool> Commit(CancellationToken cancellationToken = new CancellationToken())
        {
            if (unitOfWork.Commit())
            {
                return true;
            }

            await notificationHandler.Handle(
                Notification.Fail(
                    ApplicationConstants.ErrorCodes.SaveDataFailed,
                    ApplicationConstants.Messages.SaveDataFailed),
                cancellationToken);

            return false;
        }
    }
}
