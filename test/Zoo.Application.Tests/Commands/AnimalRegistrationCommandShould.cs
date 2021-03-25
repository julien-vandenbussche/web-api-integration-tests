namespace Zoo.Application.Tests.Commands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Administration.AnimalsRegistrationAggregate;
    using Administration.AnimalsRegistrationAggregate.Models;
    using Administration.Common;

    using Application.Commands;

    using FizzWare.NBuilder;

    using Moq;

    using NUnit.Framework;

    using Park.BearsAggregate.Models;

    [TestFixture]
    public class AnimalRegistrationCommandShould
    {
        [Test]
        public async Task CallCreatedCallbackWhenBearIsValid()
        {
            var bearCreating = Builder<BearCreating>.CreateNew()
                                                    .With(b => b.Foods = new List<int> { 2 }).Build();
            var bearDetails = Builder<BearDetails>.CreateNew().Build();
            var mockedCreatedCallback = new Mock<CreatedCallback<BearDetails>>();
            var mockedAdapter = new Mock<IAnimalsRegistrationAdapter>();
            mockedAdapter.Setup(adapter => adapter.RegisterAsync<BearCreating, BearDetails>(bearCreating))
                         .ReturnsAsync(bearDetails);
            var command = new AnimalRegistrationCommand(mockedAdapter.Object);

            await command.ExecuteAsync(bearCreating, mockedCreatedCallback.Object, null);
            
            mockedCreatedCallback.Verify(callback => callback(bearDetails), Times.Once);
        }
        
        [Test]
        public async Task CallNotCreatedCallbackWhenBearIsValid()
        {
            var wrongBearCreating = new BearCreating();
            var mockedNotCreatedCallback = new Mock<NotCreatedCallback<AnimalCreating>>();
            var command = new AnimalRegistrationCommand(null);

            await command.ExecuteAsync<BearCreating, BearDetails>(wrongBearCreating, null, mockedNotCreatedCallback.Object);
            
            mockedNotCreatedCallback.Verify(callback => callback(It.IsNotNull<BearCreatingError>()), Times.Once);
        }
    }
}