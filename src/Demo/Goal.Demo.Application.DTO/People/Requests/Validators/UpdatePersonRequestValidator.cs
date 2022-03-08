namespace Goal.Demo.Application.DTO.People.Requests.Validators
{
    public sealed class UpdatePersonRequestValidator : PersonRequestValidator<UpdatePersonRequest>
    {
        public UpdatePersonRequestValidator()
        {
            ValidateFistName();
            ValidateLastName();
            ValidateCpf();
        }
    }
}
