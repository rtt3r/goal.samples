using FluentValidation;
using Goal.Demo2.Application.Commands.Customers;
using Goal.Demo2.Infra.Crosscutting.Constants;

namespace Goal.Demo2.Application.Validations.Customers
{
    public class UpdateCustomerCommandValidator : CustomerValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            ValidateId();
            ValidateName();
            ValidateBirthDate();
        }

        protected void ValidateId()
        {
            RuleFor(c => c.CustomerId)
                .NotEmpty()
                    .WithMessage(ApplicationConstants.Messages.CustomerIdRequired)
                    .WithErrorCode(ApplicationConstants.ErrorCodes.CustomerIdRequired);
        }
    }
}
