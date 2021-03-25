namespace Zoo.Api.Tests.EndToEnd
{
    using System;
    using System.Collections;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Core;

    using FluentAssertions;

    using Infrastructure.Contracts.Veterinary;
    using Infrastructure.Contracts.Veterinary.Models;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class InfirmarySteps : Steps
    {
        private readonly ScenarioContext scenarioContext;

        public InfirmarySteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given(@"the referential have any veterinaries")]
        public void GivenTheReferentialHaveAnyVeterinaries(IEnumerable veterinaries)
        {
            this.scenarioContext.Configure<IServiceCollection>(
                services =>
                    {
                        services.RegisterMock<IVeterinaryClient>((s, mock) =>
                                                                     {
                                                                         mock.Setup(client => client.GetAsync())
                                                                             .ReturnsAsync(
                                                                                 veterinaries
                                                                                     .As<IImmutableList<Veterinary>>()
                                                                                     .ToList());
                                                                         s.AddSingleton(mock.Object);
                                                                     });
                    });
        }
        
        [Then(@"the content have veterinary informations")]
        public async Task ThenTheContentHaveVeterinaryInformation(Table expectedVeterinaryInformation)
        {
            var expectedData = expectedVeterinaryInformation.CreateSet<Veterinary>()
                                                            .Select(
                                                                item =>
                                                                    {
                                                                        item.Address = item.Address.Replace(
                                                                            "\\r\n", Environment.NewLine);
                                                                        return item;
                                                                    }).ToList();
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            var actualValue = await response.Content.ReadContentAsync(
                                  value => JsonConvert.DeserializeObject(
                                      value,
                                      expectedData.GetType()));

            actualValue.Should().BeEquivalentTo(expectedData);
        }
    }
}