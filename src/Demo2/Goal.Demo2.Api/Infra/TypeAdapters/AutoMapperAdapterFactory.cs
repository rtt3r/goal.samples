using AutoMapper;
using Goal.Infra.Crosscutting.Adapters;

namespace Goal.Demo2.Api.Infra.TypeAdapters
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
