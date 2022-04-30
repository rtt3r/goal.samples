using FluentValidation;
using Goal.Demo2.Application.Commands.Customers;
using Goal.Demo2.Infra.Crosscutting.Constants;

namespace Goal.Demo2.Application.Validations.Customers
{
    public abstract class CustomerValidator<TCommand> : AbstractValidator<TCommand>
        where TCommand : CustomerCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                    .WithMessage(ApplicationConstants.Messages.CustomerNameRequired)
                    .WithErrorCode(ApplicationConstants.ErrorCodes.CustomerNameRequired)
                .Length(2, 150)
                    .WithMessage(ApplicationConstants.Messages.CustomerNameLengthInvalid)
                    .WithErrorCode(ApplicationConstants.ErrorCodes.CustomerNameLengthInvalid);
        }

        protected void ValidateBirthDate()
        {
            RuleFor(c => c.BirthDate)
                .NotEmpty()
                    .WithMessage(ApplicationConstants.Messages.CustomerBirthDateRequired)
                    .WithErrorCode(ApplicationConstants.ErrorCodes.CustomerBirthDateRequired)
                .Must(HaveMinimumAge)
                    .WithMessage(ApplicationConstants.Messages.CustomerBirthDateLengthInvalid)
                    .WithErrorCode(ApplicationConstants.ErrorCodes.CustomerBirthDateLengthInvalid);
        }

        protected static bool HaveMinimumAge(DateTime birthDate)
            => birthDate.Date <= DateTime.Today.AddYears(-18);
    }
}
