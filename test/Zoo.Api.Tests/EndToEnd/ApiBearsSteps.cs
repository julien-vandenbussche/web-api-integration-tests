namespace Zoo.Api.Tests.EndToEnd
{
    using System.Collections;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Administration.AnimalsRegistrationAggregate.Models;

    using Core;

    using Infrastructure.Entities.Zoo;
    using Infrastructure.Store;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    using Park.BearsAggregate.Models;

    using FluentAssertions;


    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ApiBearsSteps : Steps
    {
        private readonly ScenarioContext scenarioContext;

        public ApiBearsSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given(@"the referential have any animals")]
        public void GivenTheReferentialHaveAnyAnimals(IEnumerable animals)
        {
            this.scenarioContext.ConfigureDb(
                dbContext => { dbContext.Set<Animal>().AddRange(animals.Cast<Animal>()); });
        }

        [Given(@"the referential have any classification")]
        public void GivenTheReferentialHaveAnyClassification(IEnumerable classification)
        {
            this.scenarioContext.ConfigureDb(
                context => { context.Set<Classification>().AddRange(classification.Cast<Classification>()); });
        }

        [Given(@"the referential have any families")]
        public void GivenTheReferentialHaveAnyFamilies(IEnumerable families)
        {
            this.scenarioContext.ConfigureDb(
                context => { context.Set<Family>().AddRange(families.Cast<Family>()); });
        }
        
        [Then(@"the content have restrained bears")]
        public async Task ThenTheContentHaveRestrainedAnimals(IEnumerable expectedRetrainedAnimals)
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            var actualValue = await response.Content.ReadContentAsync(
                                  value => JsonConvert.DeserializeObject(
                                      value,
                                      expectedRetrainedAnimals.GetType()));

            actualValue.Should().BeEquivalentTo(expectedRetrainedAnimals);
        }

        [Then(@"the http status code of response is (.*)")]
        public void ThenTheHttpStatusCodeOfResponseIs(HttpStatusCode expectedHttpStatusCode)
        {
            var response = this.scenarioContext.Get<HttpResponseMessage>();
            response.StatusCode.Should().Be(expectedHttpStatusCode);
        }

        [Given(@"the referential have any foods")]
        public void GivenTheReferentialHaveAnyFoods(IEnumerable foods)
        {
            this.scenarioContext.ConfigureDb(
                context => { context.Set<Food>().AddRange(foods.Cast<Food>()); });
        }
        
        [Given(@"the animal can eats")]
        public void GivenTheAnimalCanEats(IEnumerable animalCanEats)
        {
            this.scenarioContext.ConfigureDb(
                context => { context.Set<AnimalCanEat>().AddRange(animalCanEats.Cast<AnimalCanEat>()); });
        }
        
        [When(@"i would like register bear")]
        public void WhenIWouldLikeRegisterBear(Table table)
        {
            var bearCreating = table.CreateInstance<BearCreating>();
            this.scenarioContext.Set(bearCreating, "Post");
        }
        
        [Then(@"the content is")]
        public async Task ThenTheContentIs(Table table)
        {
            var response = this.ScenarioContext.Get<HttpResponseMessage>();
            Task<T> ReadContent<T>() => response.Content.ReadContentAsync<T>();
            var expectedLocation = table.Rows[0].GetString("location");
            var expectedSuccessValue = table.CreateInstance<BearDetails>();
            var successValue = await ReadContent<BearDetails>();
            var locationValue = response.Headers.Location.ToString();
            successValue.Should().BeEquivalentTo(expectedSuccessValue);
            locationValue.Should().Be(expectedLocation);
            
            this.scenarioContext.AddAssertion(
                provider =>
                    {
                        var dbContext = provider.GetRequiredService<IDbContext>();
                        var animal = dbContext.Set<Animal>()
                                              .Include(a => a.Family)
                                              .Include(a => a.AnimalEats)
                                              .ThenInclude(a => a.Food)
                                              .Single(a => a.Id == successValue.Id);
                        animal.Family.Name.Should().Be(successValue.Family);
                        animal.Name.Should().Be(successValue.Name);
                        animal.Legs.Should().Be(successValue.Legs);
                        animal.AnimalEats.Select(ae => ae.Food.Name).Should().BeEquivalentTo(successValue.Foods);
                    });
        }
    }
}