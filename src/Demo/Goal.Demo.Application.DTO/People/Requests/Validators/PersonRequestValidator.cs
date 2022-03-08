using FluentValidation;
using Goal.Infra.Crosscutting.Validations;

namespace Goal.Demo.Application.DTO.People.Requests.Validators
{
    public abstract class PersonRequestValidator<T> : AbstractValidator<T> where T : PersonRequest
    {
        protected void ValidateFistName()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("Please ensure you have entered the first name")
                .Length(2, 50).WithMessage("The first name must have between 2 and 50 characters");
        }

        protected void ValidateLastName()
        {
            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("Please ensure you have entered the last name")
                .Length(2, 50).WithMessage("The last name must have between 2 and 50 characters");
        }

        protected void ValidateCpf()
        {
            RuleFor(c => c.Cpf)
                .NotEmpty().WithMessage("Please ensure you have entered the cpf")
                .Length(11).WithMessage("The cpf must have exactly 11 characters")
                .Cpf().WithMessage("The cpf is not valid");
        }
    }
}
