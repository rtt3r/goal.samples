using AutoMapper;
using Goal.Samples.DDD.Domain.People.Aggregates;
using Ritter.Samples.Application.DTO.People.Responses;

namespace Goal.Samples.DDD.Application.TypeAdapters.Profiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<Person, PersonResponse>()
            .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf.Number));
    }
}
