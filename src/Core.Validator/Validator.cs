namespace Core.Validator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;

    public interface IValidator<in T>
    {
        bool Validate(T value, Action<IEnumerable<Error>> addErrors);
    }

    public abstract class Validator<T> : AbstractValidator<T>, IValidator<T>
    {
        protected Validator()
        {
            this.DefineRules();
        }

        public bool Validate(T value, Action<IEnumerable<Error>> addErrors)
        {
            var validationResult = base.Validate(value);
            var failures = validationResult.Errors;
            var errors = failures.Select(failure => new Error(failure.PropertyName, failure.ErrorMessage));
            addErrors(errors);
            return validationResult.IsValid;
        }

        protected abstract void OnDefineRules();

        private void DefineRules()
        {
            this.OnDefineRules();
        }
    }
}