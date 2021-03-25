namespace Zoo.Administration.AnimalsRegistrationAggregate.Models
{
    using System.Collections.Immutable;

    using Core.Validator;

    public interface IModelError
    {
        public IImmutableList<Error> Errors { get; }
    }
}