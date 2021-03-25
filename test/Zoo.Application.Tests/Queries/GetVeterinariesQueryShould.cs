namespace Zoo.Application.Tests.Queries
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;

    using Application.Queries;

    using FizzWare.NBuilder;

    using Infirmary.VeterinaryAggregate;
    using Infirmary.VeterinaryAggregate.Models;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class GetVeterinariesQueryShould 
    {
        [Test]
        public async Task CallNotFoundWhenNotHaveVeterinaries()
        {
            var mockedNotFoundCallback = new Mock<NotFoundCallback>();
            var mockedAdapter = new Mock<IVeterinaryAdapter>();
            mockedAdapter.Setup(adapter => adapter.GetAsync())
                         .ReturnsAsync(Enumerable.Empty<VeterinaryContact>().ToImmutableList());
            var query = new GetVeterinariesQuery(mockedAdapter.Object);
            
            await query.GetAsync(null, mockedNotFoundCallback.Object);
            
            mockedNotFoundCallback.Verify(notFound => notFound(), Times.Once);
        }

        [Test]
        public async Task CallFoundWhenCallGetAndHaveAnyAnimals()
        {
            var veterinaryContacts = Builder<VeterinaryContact>.CreateListOfSize(10).Build().ToImmutableList();
            var mockedFoundCallback = new Mock<FoundCallback<IImmutableList<VeterinaryContact>>>();
            var mockedAdapter = new Mock<IVeterinaryAdapter>();
            mockedAdapter.Setup(adapter => adapter.GetAsync())
                         .ReturnsAsync(veterinaryContacts);
            var query = new GetVeterinariesQuery(mockedAdapter.Object);
            
            await query.GetAsync(mockedFoundCallback.Object, null);
            
            mockedFoundCallback.Verify(found => found(veterinaryContacts), Times.Once);
        }
    }
}