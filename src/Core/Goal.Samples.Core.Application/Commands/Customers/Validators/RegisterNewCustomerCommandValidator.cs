using FluentValidation;
using Goal.Samples.Infra.Crosscutting.Constants;

namespace Goal.Samples.Core.Application.Commands.Customers.Validators;

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
                .WithMessage(ApplicationConstants.Messages.CUSTOMER_EMAIL_REQUIRED)
                .WithErrorCode(nameof(ApplicationConstants.Messages.CUSTOMER_EMAIL_REQUIRED))
            .EmailAddress()
                .WithMessage(ApplicationConstants.Messages.CUSTOMER_EMAIL_INVALID)
                .WithErrorCode(nameof(ApplicationConstants.Messages.CUSTOMER_EMAIL_INVALID));
    }
}
