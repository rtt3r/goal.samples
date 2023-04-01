using AutoMapper;
using Goal.Seedwork.Infra.Crosscutting.Adapters;

namespace Goal.Samples.CQRS.Api.Infra.TypeAdapters
{
    public class AutoMapperAdapterFactory : ITypeAdapterFactory
    {
        private readonly IMapper mapper;

        public AutoMapperAdapterFactory(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public ITypeAdapter Create() => new AutoMapperAdapter(mapper);
    }
}
