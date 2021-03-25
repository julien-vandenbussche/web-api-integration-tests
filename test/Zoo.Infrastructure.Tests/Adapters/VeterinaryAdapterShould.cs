namespace Zoo.Infrastructure.Tests.Adapters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Contracts.Veterinary;
    using Contracts.Veterinary.Models;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Infirmary.VeterinaryAggregate;
    using Infirmary.VeterinaryAggregate.Models;

    using Infrastructure.Adapters;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class VeterinaryAdapterShould
    {
        [Test]
        public async Task ReturnMappedVeterinaryContactsWhenCallGet()
        {
            var expectedVeterinaryContacts = Builder<VeterinaryContact>.CreateListOfSize(10).Build();
            var veterinaries = Builder<Veterinary>.CreateListOfSize(10).Build();
            var mockedVeterinaryClient = new Mock<IVeterinaryClient>();
            var mockedMapper = new Mock<IMapper>();
            mockedVeterinaryClient.Setup(client => client.GetAsync()).ReturnsAsync(veterinaries.ToList);
            mockedMapper.Setup(mapper => mapper.Map<List<VeterinaryContact>>(veterinaries)).Returns(expectedVeterinaryContacts.ToList);
            IVeterinaryAdapter adapter = new VeterinaryAdapter(mockedVeterinaryClient.Object, mockedMapper.Object);

            var veterinaryContacts = await adapter.GetAsync();

            veterinaryContacts.Should().BeEquivalentTo(expectedVeterinaryContacts);
        }
    }
}