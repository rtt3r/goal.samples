using FluentValidation;
using Goal.Demo2.Infra.Crosscutting.Constants;

namespace Goal.Demo2.Application.Customers.Commands.Validators
{
    public class RegisterNewCustomerCommandValidator : CustomerValidator<RegisterNewCustomerCommand>
    {
        public RegisterNewCustomerCommandValidator()
        {
            ValidateName();
            ValidateBirthDate();
            ValidateEmail();
        }

        protected void ValidateEmail()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                    .WithMessage(ApplicationConstants.Messages.CustomerEmailRequired)
                    .WithErrorCode(ApplicationConstants.ErrorCodes.CustomerEmailRequired)
                .EmailAddress()
                    .WithMessage(ApplicationConstants.Messages.CustomerEmailAddressInvalid)
                    .WithErrorCode(ApplicationConstants.ErrorCodes.CustomerEmailAddressInvalid);
        }
    }
}
