namespace Zoo.Application.Queries
{
    using System.Collections.Immutable;
    using System.Linq;

    using Park.Common.Adapters;
    using Park.Common.Models;

    public interface IGetRestrainedAnimalsQuery
    {
        void Get<TRestrainedAnimal>(FoundCallback<IImmutableList<TRestrainedAnimal>> found, NotFoundCallback notFound) where TRestrainedAnimal : AnimalRestrained, new();
    }
    
    internal sealed class GetRestrainedAnimalsQuery : IGetRestrainedAnimalsQuery
    {
        private readonly IRestrainedAnimalAdapter restrainedAnimalAdapter;

        public GetRestrainedAnimalsQuery(IRestrainedAnimalAdapter restrainedAnimalAdapter)
        {
            this.restrainedAnimalAdapter = restrainedAnimalAdapter;
        }

        public void Get<TRestrainedAnimal>(FoundCallback<IImmutableList<TRestrainedAnimal>> found, NotFoundCallback notFound) where TRestrainedAnimal : AnimalRestrained, new()
        {
            var animals = this.restrainedAnimalAdapter.Get<TRestrainedAnimal>();
            if (!animals.Any())
            {
                notFound();
                return;
            }

            found(animals);
        }
    }
}