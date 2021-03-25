namespace Zoo.Infrastructure.Adapters.Specification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Administration.AnimalsRegistrationAggregate.Models;

    using Core.Specification;

    using Entities.Zoo;

    using Park.BearsAggregate.Models;

    public class AnimalsSpecification<TAnimal> : Specification<Animal>
    {
        private const int BearCode = 3;

        private readonly int[] familyCodes;

        public AnimalsSpecification()
        {
            this.familyCodes = DefineFamily();
        }

        public override Expression<Func<Animal, bool>> ToExpression() => animal => this.familyCodes.Any(family => family == animal.FamilyId);

        protected override void OnAddRelation(AddRelationship<Animal> addRelationship)
        {
            base.OnAddRelation(addRelationship);
            addRelationship(Relationship<Animal>.Create(animal => animal.Family));
        }

        private static int[] DefineFamily()
        {
            var type = typeof(TAnimal);
            var result = new List<int>();
            if (type == typeof(BearRestrained) || type == typeof(BearDetails) || type == typeof(BearCreating))
            {
                result.Add(BearCode);
            }

            return result.ToArray();
        }
    }
}