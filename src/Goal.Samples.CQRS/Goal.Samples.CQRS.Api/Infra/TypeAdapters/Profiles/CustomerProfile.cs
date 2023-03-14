using AutoMapper;
using Goal.Samples.CQRS.Domain.Customers.Aggregates;
using Goal.Samples.CQRS.Model.Customers;

namespace Goal.Samples.CQRS.Api.Infra.TypeAdapters.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerModel>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
