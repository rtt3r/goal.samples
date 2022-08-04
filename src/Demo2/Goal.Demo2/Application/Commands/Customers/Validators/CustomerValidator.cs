using FluentValidation;
using Goal.Demo2.Application.Commands.Customers;
using Goal.Demo2.Infra.Crosscutting.Constants;

namespace Goal.Demo2.Application.Commands.Customers.Validators
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

        protected void ValidateBirthdate()
        {
            RuleFor(c => c.Birthdate)
                .NotEmpty()
                    .WithMessage(ApplicationConstants.Messages.CustomerBirthdateRequired)
                    .WithErrorCode(ApplicationConstants.ErrorCodes.CustomerBirthdateRequired)
                .Must(HaveMinimumAge)
                    .WithMessage(ApplicationConstants.Messages.CustomerBirthdateLengthInvalid)
                    .WithErrorCode(ApplicationConstants.ErrorCodes.CustomerBirthdateLengthInvalid);
        }

        protected static bool HaveMinimumAge(DateTime Birthdate)
            => Birthdate.Date <= DateTime.Today.AddYears(-18);
    }
}
