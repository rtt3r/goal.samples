using FluentValidation;
using Goal.Demo2.Api.Application.Commands.Customers;

namespace Goal.Demo2.Api.Application.Validations.Customers
{
    public class UpdateCustomerCommandValidation : CustomerValidation<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidation()
        {
            ValidateId();
            ValidateName();
            ValidateBirthDate();
        }

        protected void ValidateId()
        {
            RuleFor(c => c.AggregateId)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}
