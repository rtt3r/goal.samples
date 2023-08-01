using AutoMapper;
using Goal.Samples.CQRS.Domain.Customers.Aggregates;
using CustomerModel = Goal.Samples.CQRS.Model.Customers.Customer;

namespace Goal.Samples.CQRS.Application.TypeAdapters.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerModel>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id));
    }
}
