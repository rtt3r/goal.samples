using FluentValidation.Results;
using Goal.Demo2.Application.Customers.Commands.Validators;
using Goal.Demo2.Domain.Customers.Aggregates;
using Goal.Demo2.Domain.Customers.Events;
using Goal.Demo2.Infra.Crosscutting.Constants;
using Goal.Demo2.Model.Customers;
using Goal.Seedwork.Application.Extensions;
using Goal.Seedwork.Application.Handlers;
using Goal.Seedwork.Domain;
using Goal.Seedwork.Domain.Commands;
using Goal.Seedwork.Infra.Crosscutting.Adapters;

namespace Goal.Demo2.Application.Customers.Commands
{
    public class CustomerCommandHandler : CommandHandler,
        ICommandHandler<RegisterNewCustomerCommand, ICommandResult<CustomerModel>>,
        ICommandHandler<UpdateCustomerCommand, ICommandResult>,
        ICommandHandler<RemoveCustomerCommand, ICommandResult>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITypeAdapter typeAdapter;

        public CustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IBusHandler busHandler,
            INotificationHandler notificationHandler,
            ITypeAdapter typeAdapter,
            ILogger<CustomerCommandHandler> logger)
            : base(notificationHandler, busHandler, logger)
        {
            this.customerRepository = customerRepository;
            this.unitOfWork = unitOfWork;
            this.typeAdapter = typeAdapter;
        }

        public async Task<ICommandResult<CustomerModel>> Handle(RegisterNewCustomerCommand command, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await command.ValidateCommandAsync(
                new RegisterNewCustomerCommandValidator(),
                cancellationToken);

            if (!validationResult.IsValid)
            {
                await NotifyContractViolations(validationResult, cancellationToken);
                return CommandResult.ContractViolation<CustomerModel>(default);
            }

            var customer = new Customer(command.Name, command.Email, command.BirthDate);

            if (await customerRepository.GetByEmail(customer.Email) != null)
            {
                await NotifyDomainViolation(
                    ApplicationConstants.ErrorCodes.CustomerEmailDuplicated,
                    ApplicationConstants.Messages.CustomerEmailDuplicated,
                    cancellationToken);

                return CommandResult.DomainViolation<CustomerModel>(default);
            }

            await customerRepository.AddAsync(customer, cancellationToken);

            if (await Commit(cancellationToken))
            {
                await RaiseEvent(new CustomerRegisteredEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));

                return CommandResult.Success(
                    typeAdapter.ProjectAs<CustomerModel>(customer));
            }

            return CommandResult.DomainViolation<CustomerModel>(default);
        }

        public async Task<ICommandResult> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await command.ValidateCommandAsync(
                new UpdateCustomerCommandValidator(),
                cancellationToken);

            if (!validationResult.IsValid)
            {
                await NotifyContractViolations(validationResult, cancellationToken);
                return CommandResult.ContractViolation();
            }

            Customer customer = await customerRepository.LoadAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                await NotifyDomainViolation(
                    ApplicationConstants.ErrorCodes.CustomerNotFound,
                    ApplicationConstants.Messages.CustomerNotFound,
                    cancellationToken);

                return CommandResult.DomainViolation();
            }

            Customer existingCustomer = await customerRepository.GetByEmail(customer.Email);

            if (existingCustomer != null && existingCustomer != customer)
            {
                await NotifyDomainViolation(
                    ApplicationConstants.ErrorCodes.CustomerEmailDuplicated,
                    ApplicationConstants.Messages.CustomerEmailDuplicated,
                    cancellationToken);

                return CommandResult.DomainViolation();
            }

            customer.UpdateName(command.Name);
            customer.UpdateBirthDate(command.BirthDate);

            customerRepository.Update(existingCustomer);

            if (await Commit(cancellationToken))
            {
                await busHandler.RaiseEvent(new CustomerUpdatedEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));
                return CommandResult.Success();
            }

            return CommandResult.DomainViolation();
        }

        public async Task<ICommandResult> Handle(RemoveCustomerCommand command, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await command.ValidateCommandAsync(
                new RemoveCustomerCommandValidator(),
                cancellationToken);

            if (!validationResult.IsValid)
            {
                await NotifyContractViolations(validationResult, cancellationToken);
                return CommandResult.ContractViolation();
            }

            Customer customer = await customerRepository.LoadAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                await NotifyDomainViolation(
                    ApplicationConstants.ErrorCodes.CustomerNotFound,
                    ApplicationConstants.Messages.CustomerNotFound,
                    cancellationToken);

                return CommandResult.DomainViolation();
            }

            customerRepository.Remove(customer);

            if (await Commit(cancellationToken))
            {
                await busHandler.RaiseEvent(new CustomerRemovedEvent(command.CustomerId));
                return CommandResult.Success();
            }

            return CommandResult.DomainViolation();
        }

        private async Task<bool> Commit(CancellationToken cancellationToken = new CancellationToken())
        {
            if (unitOfWork.Commit())
            {
                return true;
            }

            await NotifyFail(
                ApplicationConstants.ErrorCodes.SaveDataFailed,
                ApplicationConstants.Messages.SaveDataFailed,
                cancellationToken);

            return false;
        }
    }
}
