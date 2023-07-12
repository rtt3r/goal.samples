using FluentValidation.Results;
using Goal.Samples.DDD.Application.DTO.People.Requests;
using Goal.Samples.DDD.Application.DTO.People.Requests.Validators;
using Goal.Samples.DDD.Application.DTO.People.Responses;
using Goal.Samples.DDD.Domain;
using Goal.Samples.DDD.Domain.People.Aggregates;
using Goal.Samples.Infra.Crosscutting.Exceptions;
using Goal.Seedwork.Application.Extensions;
using Goal.Seedwork.Application.Services;
using Goal.Seedwork.Infra.Crosscutting.Adapters;
using Goal.Seedwork.Infra.Crosscutting.Collections;
using Goal.Seedwork.Infra.Crosscutting.Trying;

namespace Goal.Samples.DDD.Application.People;

public class PersonAppService : AppService, IPersonAppService
{
    private readonly IDddUnitOfWork uow;
    private readonly ITypeAdapter adapter;

    public PersonAppService(
        IDddUnitOfWork uow,
        ITypeAdapter adapter)
        : base()
    {
        this.uow = uow;
        this.adapter = adapter;
    }

    public async Task<IPagedCollection<PersonResponse>> FindPaginatedAsync(IPageSearch pageSearch)
    {
        IPagedCollection<Person> people = await uow.People.QueryAsync(pageSearch);
        return adapter.ProjectAsPagedCollection<PersonResponse>(people);
    }

    public async Task<PersonResponse> GetPersonAsync(string id)
    {
        Person person = await uow.People.LoadAsync(id)
            ?? throw new NotFoundException("Pessoa não encontrada");

        return adapter.ProjectAs<PersonResponse>(person);
    }

    public async Task<Try<ApplicationException, PersonResponse>> AddPerson(AddPersonRequest request)
    {
        ValidationResult validationResult = await new AddPersonRequestValidator().ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            IEnumerable<string> propertyErrors = validationResult.Errors
                .GroupBy(p => p.PropertyName)
                .Select(g => $"{g.Key}: {string.Join(", ", g.Select(e => e.ErrorMessage))}");

            throw new BusinessException(string.Join("; ", propertyErrors.ToArray()));
        }

        if (await uow.People.AnyAsync(PersonSpecifications.MatchCpf(request.Cpf)))
        {
            throw new BusinessException("CPF duplicado");
        }

        var person = Person.CreatePerson(
            request.FirstName,
            request.LastName,
            request.Cpf);

        await uow.People.AddAsync(person);
        await uow.SaveAsync();

        return adapter.ProjectAs<PersonResponse>(person);
    }

    public async Task<Try<ApplicationException, PersonResponse>> UpdatePerson(string id, UpdatePersonRequest request)
    {
        ValidationResult validationResult = await new UpdatePersonRequestValidator().ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            throw new BusinessException(validationResult.Errors.First().ToString());
        }

        Person person = await uow.People.LoadAsync(id)
            ?? throw new NotFoundException("Pessoa não encontrada");

        if (await uow.People.AnyAsync(
            !PersonSpecifications.MatchId(person.Id)
            && PersonSpecifications.MatchCpf(person.Cpf.Number)))
        {
            throw new BusinessException("CPF duplicado");
        }

        uow.People.Update(person);
        await uow.SaveAsync();

        return adapter.ProjectAs<PersonResponse>(person);
    }

    public async Task<Try<ApplicationException, bool>> DeletePerson(string id)
    {
        Person person = await uow.People.LoadAsync(id)
            ?? throw new NotFoundException("Pessoa não encontrada");

        uow.People.Remove(person);
        return await uow.SaveAsync();
    }
}
