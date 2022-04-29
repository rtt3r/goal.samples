using AutoMapper;
using Goal.Demo2.Domain.Aggregates.Customers;
using Goal.Demo2.Model.Customers;

namespace Goal.Demo2.Api.Infra.TypeAdapters.Profiles
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
