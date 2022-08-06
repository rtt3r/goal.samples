using FluentValidation;
using Goal.Demo2.Infra.Crosscutting.Constants;

namespace Goal.Demo2.Application.Commands.Customers.Validators
{
    public class UpdateCustomerCommandValidator : CustomerValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            ValidateId();
            ValidateName();
            ValidateBirthdate();
        }

        protected void ValidateId()
        {
            RuleFor(c => c.CustomerId)
                .NotEmpty()
                    .WithMessage(ApplicationConstants.Messages.CUSTOMER_ID_REQUIRED)
                    .WithErrorCode(nameof(ApplicationConstants.Messages.CUSTOMER_ID_REQUIRED));
        }
    }
}
