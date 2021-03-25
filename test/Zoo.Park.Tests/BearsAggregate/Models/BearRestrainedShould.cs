namespace Zoo.Park.Tests.BearsAggregate.Models
{
    using FluentAssertions;

    using NUnit.Framework;

    using Park.BearsAggregate.Models;

    [TestFixture]
    public class BearRestrainedShould
    {
        [Test]
        public void HaveFamilyEqualToBear()
        {
            const string expectedFamily = "Bear";
            var bear = new BearRestrained();
            bear.Family.Should().Be(expectedFamily);
        }
    }
}