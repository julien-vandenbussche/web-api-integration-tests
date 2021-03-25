namespace Zoo.Administration.Tests.AnimalsRegistrationAggregate.Models
{
    using System.Collections.Generic;

    using Administration.AnimalsRegistrationAggregate.Models;

    using FizzWare.NBuilder;

    using NUnit.Framework;

    using FluentAssertions;

    [TestFixture]
    public class BearCreatingErrorShould
    {
        [Test]
        public void RewritePropertiesOfBearCreatingWhenCreateInstance()
        {
            var model = Builder<BearCreating>.CreateNew()
                                             .With(m => m.Foods = new List<int> { 5, 3, 2 }).Build();
            
            var errorModel = new BearCreatingError(model);
            
            errorModel.Should().BeEquivalentTo(model);
            errorModel.Errors.Should().NotBeNull();
        }
    }
}