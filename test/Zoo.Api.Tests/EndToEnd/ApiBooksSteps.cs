namespace Zoo.Api.Tests.EndToEnd
{
    using System.Collections;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using AxaFrance.Extensions.ServiceModel;

    using Core;

    using FluentAssertions;

    using Infrastructure.BookService;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    [Binding]
    public class ApiBooksSteps : Steps
    {
        private readonly ScenarioContext scenarioContext;

        public ApiBooksSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given(@"the referential have any books")]
        public void GivenTheReferentialHaveAnyBooks(IEnumerable books)
        {
            this.scenarioContext.Configure<IServiceCollection>(
                services =>
                    {
                        var mockWrapper = new Mock<IServiceClientWrapper<BookServiceChannel>>();
                        var mockChannel = new Mock<BookServiceChannel>();
                        mockWrapper.Setup(wrapper => wrapper.Channel)
                                   .Returns(mockChannel.Object);
                        services.AddSingleton(mockWrapper.Object);
                        mockChannel.Setup(channel => channel.GetAllBooksAsync(It.IsNotNull<GetAllBooksRequest>()))
                                   .ReturnsAsync(new GetAllBooksResponse(books.Cast<Book>().ToArray()));
                    });
        }
        
        [Then(@"the content have books of bear")]
        public async Task ThenTheContentHaveBooksOfBear(IEnumerable expectedBooks)
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            var actualValue = await response.Content.ReadContentAsync(
                                  value => JsonConvert.DeserializeObject(
                                      value,
                                      expectedBooks.GetType()));

            actualValue.Should().BeEquivalentTo(expectedBooks);
        }
    }
}