using FluentValidation;
using Goal.Samples.Infra.Crosscutting.Constants;

namespace Goal.Samples.CQRS.Application.Commands.Customers.Validators;

public class RegisterNewCustomerCommandValidator : CustomerValidator<RegisterNewCustomerCommand>
{
    public RegisterNewCustomerCommandValidator()
    {
        ValidateName();
        ValidateBirthdate();
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
