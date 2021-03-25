namespace Zoo.Administration.AnimalsRegistrationAggregate.Validators
{
    using Core.Validator;

    using FluentValidation;

    using Models;

    public sealed class BearCreatingValidator : Validator<BearCreating>
    {
        protected override void OnDefineRules()
        {
            this.RuleFor(bear => bear.Name).NotEmpty().MaximumLength(50);
            this.RuleFor(bear => bear.Legs).LessThanOrEqualTo(4);
            this.RuleFor(bear => bear.Foods).NotEmpty();
            
        }
    }
}