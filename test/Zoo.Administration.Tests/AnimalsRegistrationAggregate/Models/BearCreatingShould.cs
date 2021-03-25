using NUnit.Framework;

namespace Zoo.Administration.Tests.AnimalsRegistrationAggregate.Models
{
    using System;
    using System.Collections.Generic;

    using Administration.AnimalsRegistrationAggregate.Models;

    using Core.Validator;

    using Moq;

    using FluentAssertions;

    [TestFixture]
    public class BearCreatingShould
    {
        [Test]
        public void ReturnErrorModelWhenBearIsNotValid()
        {
            var mockedValidator = new Mock<IValidator<BearCreating>>();
            var model = new BearCreating(mockedValidator.Object);
            mockedValidator.Setup(v => v.Validate(model, It.IsNotNull<Action<IEnumerable<Error>>>()))
                           .Returns(false);

            var actualValue = model.Validate();

            actualValue.Should().BeOfType<BearCreatingError>();
        }

        [Test]
        public void ReturnModelWhenBearIsValid()
        {
            var mockedValidator = new Mock<IValidator<BearCreating>>();
            var model = new BearCreating(mockedValidator.Object);
            mockedValidator.Setup(v => v.Validate(model, It.IsNotNull<Action<IEnumerable<Error>>>()))
                           .Returns(true);

            var actualValue = model.Validate();

            actualValue.Should().Be(model);
        }
    }
}