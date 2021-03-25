namespace Zoo.Api.Tests.EndToEnd
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Core;

    using Microsoft.AspNetCore.Authentication.JwtBearer;

    using Newtonsoft.Json.Linq;

    using TechTalk.SpecFlow;

    [Binding]
    public class StepsCommon
    {
        private readonly ScenarioContext scenarioContext;

        public StepsCommon(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given(@"I'm (.*)")]
        public void Im(string role)
        {
            var claims = this.scenarioContext.Get<List<Claim>>();
            claims.Add(new Claim(ClaimTypes.Role, role));
            this.scenarioContext.Set(claims);
        }

        [Given(@"The current context is (.*)")]
        public void TheCurrentContextIs(string contextName)
        {
            this.scenarioContext.Set(contextName, ScenarioContextExtensions.CurrentContextName);
        }

        [When(@"i call the http resource '(.*)' with (.*) http method")]
        public async Task WhenICallTheHttpResourceWithHttpMethod(string resource, HttpMethod httpMethod)
        {
            this.scenarioContext.TryGetValue("Post", out var postingValue);
            HttpContent httpContent = null;
            if (postingValue != null)
            {
                httpContent = new StringContent(
                    JObject.FromObject(postingValue).ToString(),
                    Encoding.UTF8,
                    "application/json");
            }

            var webFactory = this.scenarioContext.Get<WebFactory<Startup>>();
            using var client = webFactory.CreateClient();
            var token = webFactory.GetBearerToken(this.scenarioContext.Get<List<Claim>>());
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"{JwtBearerDefaults.AuthenticationScheme} {token}");
            }

            var response = await client.SendAsync(
                               new HttpRequestMessage(httpMethod, $"{resource}")
                                   {
                                       Content = httpContent
                                   });
            this.scenarioContext.Set(response);
        }
    }
}