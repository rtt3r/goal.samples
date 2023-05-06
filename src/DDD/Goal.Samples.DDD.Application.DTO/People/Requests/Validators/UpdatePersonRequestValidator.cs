using FluentValidation;
using Goal.Samples.Infra.Crosscutting.Constants;
using Goal.Seedwork.Infra.Crosscutting.Validations.Fluent;

namespace Goal.Samples.DDD.Application.DTO.People.Requests.Validators;

public class UpdatePersonRequestValidator : AbstractValidator<UpdatePersonRequest>
{
    public UpdatePersonRequestValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty()
                .WithMessage(ApplicationConstants.Messages.PERSON_FIRST_NAME_REQUIRED)
                .WithErrorCode(nameof(ApplicationConstants.Messages.PERSON_FIRST_NAME_REQUIRED))
            .MaximumLength(50)
                .WithMessage(ApplicationConstants.Messages.PERSON_FIRST_NAME_MAXIMUM_LENGTH)
                .WithErrorCode(nameof(ApplicationConstants.Messages.PERSON_FIRST_NAME_MAXIMUM_LENGTH));

        RuleFor(e => e.LastName)
            .NotEmpty()
                .WithMessage(ApplicationConstants.Messages.PERSON_LAST_NAME_REQUIRED)
                .WithErrorCode(nameof(ApplicationConstants.Messages.PERSON_LAST_NAME_REQUIRED))
            .MaximumLength(50)
                .WithMessage(ApplicationConstants.Messages.PERSON_LAST_NAME_MAXIMUM_LENGTH)
                .WithErrorCode(nameof(ApplicationConstants.Messages.PERSON_LAST_NAME_MAXIMUM_LENGTH));

        RuleFor(e => e.Cpf)
            .NotEmpty()
                .WithMessage(ApplicationConstants.Messages.PERSON_CPF_REQUIRED)
                .WithErrorCode(nameof(ApplicationConstants.Messages.PERSON_CPF_REQUIRED))
            .Cpf()
                .WithMessage(ApplicationConstants.Messages.PERSON_CPF_INVALID)
                .WithErrorCode(nameof(ApplicationConstants.Messages.PERSON_CPF_INVALID));
    }
}
