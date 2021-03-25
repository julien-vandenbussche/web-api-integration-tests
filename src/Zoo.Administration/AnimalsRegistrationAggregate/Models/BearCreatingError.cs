namespace Zoo.Administration.AnimalsRegistrationAggregate.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    using Core.Validator;

    public sealed class BearCreatingError : BearCreating, IModelError
    {
        private readonly List<Error> errors;

        public BearCreatingError(BearCreating bearCreating, params Error[] errors)
            : base(bearCreating)
        {
            this.errors = new List<Error>();
            this.errors.AddRange(errors);
        }

        public IImmutableList<Error> Errors => this.errors.ToImmutableList();

        internal Action<IEnumerable<Error>> AddErrors => this.errors.AddRange;
    }
}