using AutoMapper;
using Goal.Samples.Core.Domain.Customers.Aggregates;
using CustomerModel = Goal.Samples.Core.Model.Customers.Customer;

namespace Goal.Samples.Core.Application.TypeAdapters.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerModel>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id));
    }
}
