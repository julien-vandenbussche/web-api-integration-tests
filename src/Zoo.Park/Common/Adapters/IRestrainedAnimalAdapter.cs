namespace Zoo.Park.Common.Adapters
{
    using System.Collections.Immutable;

    using Models;

    public interface IRestrainedAnimalAdapter
    {
        IImmutableList<TRestrainedAnimal> Get<TRestrainedAnimal>() where TRestrainedAnimal : AnimalRestrained, new();
    }
}