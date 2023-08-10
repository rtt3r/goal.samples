using FluentValidation;
using Goal.Samples.Infra.Crosscutting.Constants;

namespace Goal.Samples.Core.Application.Commands.Customers.Validators;

public abstract class CustomerValidator<TCommand> : AbstractValidator<TCommand>
    where TCommand : CustomerCommand
{
    protected void ValidateName()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
                .WithMessage(ApplicationConstants.Messages.CUSTOMER_NAME_REQUIRED)
                .WithErrorCode(nameof(ApplicationConstants.Messages.CUSTOMER_NAME_REQUIRED))
            .Length(2, 150)
                .WithMessage(ApplicationConstants.Messages.CUSTOMER_NAME_LENGTH_INVALID)
                .WithErrorCode(nameof(ApplicationConstants.Messages.CUSTOMER_NAME_LENGTH_INVALID));
    }

    protected void ValidateBirthDate()
    {
        RuleFor(c => c.BirthDate)
            .NotEmpty()
                .WithMessage(ApplicationConstants.Messages.CUSTOMER_BIRTHDATE_REQUIRED)
                .WithErrorCode(nameof(ApplicationConstants.Messages.CUSTOMER_BIRTHDATE_REQUIRED))
            .Must(HaveMinimumAge)
                .WithMessage(ApplicationConstants.Messages.CUSTOMER_BIRTHDATE_LENGTH_INVALID)
                .WithErrorCode(nameof(ApplicationConstants.Messages.CUSTOMER_BIRTHDATE_LENGTH_INVALID));
    }

    protected static bool HaveMinimumAge(DateTime BirthDate)
        => BirthDate.Date <= DateTime.Today.AddYears(-18);
}
