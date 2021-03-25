namespace Zoo.Administration.AnimalsRegistrationAggregate.Models
{
    using Common;

    using Core.Validator;

    using Validators;

    public class BearCreating : AnimalCreating
    {
        private readonly IValidator<BearCreating> validator;

        public BearCreating() : this(new BearCreatingValidator())
        {
        }
        
        protected BearCreating(BearCreating bearCreating)
            : this(bearCreating.validator)
        {
            this.Name = bearCreating.Name;
            foreach (var food in bearCreating.Foods)
            {
                this.Foods.Add(food);
            }

            this.Legs = bearCreating.Legs;
        }

        internal BearCreating(IValidator<BearCreating> validator)
        {
            this.validator = validator;
        }

        public override string Family => "Bear";

        public override AnimalCreating Validate()
        {
            var bearCreatingError = new BearCreatingError(this);
            var valid = this.validator.Validate(this, bearCreatingError.AddErrors);
            return valid ? this : bearCreatingError;
        }
    }
}
