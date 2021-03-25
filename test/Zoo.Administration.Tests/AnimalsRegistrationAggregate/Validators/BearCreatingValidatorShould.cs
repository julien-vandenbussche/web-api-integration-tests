namespace Zoo.Administration.Tests.AnimalsRegistrationAggregate.Validators
{
    using System.Collections.Generic;

    using Administration.AnimalsRegistrationAggregate.Models;
    using Administration.AnimalsRegistrationAggregate.Validators;

    using Core.Validator;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class BearCreatingValidatorShould
    {
        [Test]
        public void ReturnFalseWhenCallValidateAndNameOfBearIsNullOrEmpty()
        {
            var actualErrors = new List<Error>();
            var bear = Builder<BearCreating>.CreateNew().With(m => m.Name = null).Build();
            IValidator<BearCreating> validator = new BearCreatingValidator();

            var actual = validator.Validate(bear, actualErrors.AddRange);

            actual.Should().BeFalse();
            actualErrors.Should()
                        .ContainSingle(error => error.PropertyName == "Name" && error.ErrorMessage == "'Name' ne doit pas être vide.");
        }
        
        [Test]
        public void ReturnFalseWhenCallValidateAndLegsOfBearIsGreaterThan4()
        {
            var actualErrors = new List<Error>();
            var bear = Builder<BearCreating>.CreateNew().With(m => m.Legs = 5).Build();
            IValidator<BearCreating> validator = new BearCreatingValidator();

            var actual = validator.Validate(bear, actualErrors.AddRange);

            actual.Should().BeFalse();
            actualErrors.Should()
                        .ContainSingle(error => error.PropertyName == "Legs" && error.ErrorMessage == "'Legs' doit être plus petit ou égal à '4'.");
        }
        
        [Test]
        public void ReturnFalseWhenCallValidateAndFoodsOfBearIsNullOrEmpty()
        {
            var actualErrors = new List<Error>();
            var bear = Builder<BearCreating>.CreateNew().With(m => m.Foods = null).Build();
            IValidator<BearCreating> validator = new BearCreatingValidator();

            var actual = validator.Validate(bear, actualErrors.AddRange);

            actual.Should().BeFalse();
            actualErrors.Should()
                        .ContainSingle(error => error.PropertyName == "Foods" && error.ErrorMessage == "'Foods' ne doit pas être vide.");
        }
    }
}