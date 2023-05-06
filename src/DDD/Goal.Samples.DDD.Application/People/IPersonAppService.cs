using Goal.Samples.DDD.Application.DTO.People.Requests;
using Goal.Samples.DDD.Application.DTO.People.Responses;
using Goal.Seedwork.Application.Services;
using Goal.Seedwork.Infra.Crosscutting.Collections;
using Goal.Seedwork.Infra.Crosscutting.Trying;

namespace Goal.Samples.DDD.Application.People;

public interface IPersonAppService : IAppService
{
    Task<Try<ApplicationException, PersonResponse>> AddPerson(AddPersonRequest request);
    Task<Try<ApplicationException, PersonResponse>> UpdatePerson(string id, UpdatePersonRequest request);
    Task<Try<ApplicationException, bool>> DeletePerson(string id);
    Task<IPagedCollection<PersonResponse>> FindPaginatedAsync(IPageSearch pageSearch);
    Task<PersonResponse> GetPersonAsync(string id);
}
