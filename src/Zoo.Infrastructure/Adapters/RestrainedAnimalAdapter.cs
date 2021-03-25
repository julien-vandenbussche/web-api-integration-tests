namespace Zoo.Infrastructure.Adapters
{
    using System.Collections.Immutable;

    using AutoMapper;

    using Park.Common.Adapters;
    using Park.Common.Models;

    using Specification;

    using Store;

    internal sealed class RestrainedAnimalAdapter : IRestrainedAnimalAdapter
    {
        private readonly IReader reader;

        private readonly IMapper mapper;

        public RestrainedAnimalAdapter(IReader reader, IMapper mapper)
        {
            this.reader = reader;
            this.mapper = mapper;
        }
        
        public IImmutableList<TRestrainedAnimal> Get<TRestrainedAnimal>() where TRestrainedAnimal : AnimalRestrained, new()
        {
            var query = this.reader.Get(new AnimalsSpecification<TRestrainedAnimal>());
            return this.mapper.ProjectTo<TRestrainedAnimal>(query).ToImmutableList();
        }
    }
}