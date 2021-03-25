namespace Zoo.Park.Tests.BearsAggregate.Models
{
    using FluentAssertions;

    using NUnit.Framework;

    using Park.BearsAggregate.Models;

    [TestFixture]
    public class BearDetailsShould
    {
        [Test]
        public void HaveFamilyEqualToBear()
        {
            const string expectedFamily = "Bear";
            var bear = new BearDetails();
            bear.Family.Should().Be(expectedFamily);
        }
    }
}