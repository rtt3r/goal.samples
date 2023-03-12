using AutoMapper;
using Goal.Seedwork.Infra.Crosscutting.Adapters;

namespace Goal.Samples.CQRS.Infra.TypeAdapters
{
    public class AutoMapperAdapter : ITypeAdapter
    {
        private readonly IMapper mapper;

        public AutoMapperAdapter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new() => mapper.Map<TSource, TTarget>(source);

        public TTarget Adapt<TTarget>(object source) where TTarget : class, new() => mapper.Map<TTarget>(source);
    }
}
