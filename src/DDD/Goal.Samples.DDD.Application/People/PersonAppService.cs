using FluentValidation.Results;
using Goal.Samples.DDD.Application.DTO.People.Requests;
using Goal.Samples.DDD.Application.DTO.People.Requests.Validators;
using Goal.Samples.DDD.Application.DTO.People.Responses;
using Goal.Samples.DDD.Domain.People.Aggregates;
using Goal.Samples.DDD.Infra.Data;
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
    private readonly IPersonRepository personRepository;
    private readonly ITypeAdapter adapter;

    public PersonAppService(
        IDddUnitOfWork uow,
        IPersonRepository personRepository,
        ITypeAdapter adapter)
        : base()
    {
        this.uow = uow;
        this.personRepository = personRepository;
        this.adapter = adapter;
    }

    public async Task<IPagedCollection<PersonResponse>> FindPaginatedAsync(IPageSearch pageSearch)
    {
        IPagedCollection<Person> people = await personRepository.QueryAsync(pageSearch);
        return adapter.ProjectAsPagedCollection<PersonResponse>(people);
    }

    public async Task<PersonResponse> GetPersonAsync(string id)
    {
        Person person = await personRepository.LoadAsync(id)
            ?? throw new NotFoundException("Pessoa não encontrada");

        return adapter.ProjectAs<PersonResponse>(person);
    }

    public async Task<Try<ApplicationException, PersonResponse>> AddPerson(AddPersonRequest request)
    {
        ValidationResult validationResult = await new AddPersonRequestValidator().ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            throw new BusinessException(validationResult.Errors.First().ToString());
        }

        if (await personRepository.AnyAsync(PersonSpecifications.MatchCpf(request.Cpf)))
        {
            throw new BusinessException("CPF duplicado");
        }

        var person = Person.CreatePerson(
            request.FirstName,
            request.LastName,
            request.Cpf);

        await personRepository.AddAsync(person);
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

        Person person = await personRepository.LoadAsync(id)
            ?? throw new NotFoundException("Pessoa não encontrada");

        if (await personRepository.AnyAsync(
            !PersonSpecifications.MatchId(person.Id)
            && PersonSpecifications.MatchCpf(person.Cpf.Number)))
        {
            throw new BusinessException("CPF duplicado");
        }

        personRepository.Update(person);
        await uow.SaveAsync();

        return adapter.ProjectAs<PersonResponse>(person);
    }

    public async Task<Try<ApplicationException, bool>> DeletePerson(string id)
    {
        Person person = await personRepository.LoadAsync(id)
            ?? throw new NotFoundException("Pessoa não encontrada");

        personRepository.Remove(person);
        return await uow.SaveAsync();
    }
}
