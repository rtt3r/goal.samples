using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Goal.Application.Seedwork.Extensions;
using Goal.Application.Seedwork.Services;
using Goal.Demo.Application.DTO.People.Requests;
using Goal.Demo.Application.DTO.People.Requests.Validators;
using Goal.Demo.Application.DTO.People.Responses;
using Goal.Demo.Domain.Aggregates.People;
using Goal.Domain.Seedwork;
using Goal.Infra.Crosscutting.Adapters;
using Goal.Infra.Crosscutting.Collections;
using Goal.Infra.Crosscutting.Exceptions;

namespace Goal.Demo.Application.People
{
    public class PersonAppService : AppService, IPersonAppService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPersonRepository personRepository;
        private readonly ITypeAdapter adapter;

        public PersonAppService(
            IUnitOfWork unitOfWork,
            IPersonRepository personRepository,
            ITypeAdapter adapter)
            : base()
        {
            this.unitOfWork = unitOfWork;
            this.personRepository = personRepository;
            this.adapter = adapter;
        }

        public async Task<IPagedCollection<PersonResponse>> FindPaginatedAsync(Pagination pagination)
        {
            IPagedCollection<Person> people = await personRepository.QueryAsync(pagination);
            return adapter.ProjectAsPagedCollection<PersonResponse>(people);
        }

        public async Task<PersonResponse> GetPersonAsync(string id)
        {
            Person person = await personRepository.LoadAsync(id)
                ?? throw new NotFoundException("Pessoa não encontrada");

            return adapter.ProjectAs<PersonResponse>(person);
        }

        public async Task<PersonResponse> AddPerson(AddPersonRequest request)
        {
            ValidationResult result = new AddPersonRequestValidator().Validate(request);

            if (!result.IsValid)
            {
                throw new BusinessException(result.Errors.First().ToString());
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

            if (unitOfWork.Commit())
            {
                return adapter.ProjectAs<PersonResponse>(person);
            }

            return null;
        }

        public async Task<PersonResponse> UpdatePerson(string id, UpdatePersonRequest request)
        {
            ValidationResult result = new UpdatePersonRequestValidator().Validate(request);

            if (!result.IsValid)
            {
                throw new BusinessException(result.Errors.First().ToString());
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

            if (unitOfWork.Commit())
            {
                return adapter.ProjectAs<PersonResponse>(person);
            }

            return null;
        }

        public async Task<bool> DeletePerson(string id)
        {
            Person person = await personRepository.LoadAsync(id)
                ?? throw new NotFoundException("Pessoa não encontrada");

            personRepository.Remove(person);

            if (unitOfWork.Commit())
            {
                return true;
            }

            return true;
        }
    }
}
