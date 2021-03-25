namespace Zoo.Api.Tests.EndToEnd.Core
{
    using EndToEnd;

    using Humanizer;

    using Infrastructure.Entities.Parameters;
    using Infrastructure.Entities.Zoo;
    using Infrastructure.Store;

    using TechTalk.SpecFlow;

    internal static class ScenarioContextExtensions
    {
        public const string CurrentContextName = "CurrentContextName";

        public static void AddAssertion(this ScenarioContext scenarioContext, Assertion assertion)
        {
            if (scenarioContext.TryGetValue(out Assertion currentAssertion))
            {
                currentAssertion += assertion;
                scenarioContext.Set(currentAssertion);
                return;
            }

            scenarioContext.Set(assertion);
        }

        public static void Configure<T>(this ScenarioContext scenarioContext, Configure<T> configure)
        {
            if (scenarioContext.TryGetValue(out Configure<T> currentConfigure))
            {
                currentConfigure += configure;
                scenarioContext.Set(currentConfigure);
                return;
            }

            scenarioContext.Set(configure);
        }   
        
        public static void ConfigureDb(this ScenarioContext scenarioContext,  Configure<IDbContext> configure)
        {
            Configure<TOrigin> Convert<TOrigin>(Configure<IDbContext> additionalConfig)
            {
                var origin = scenarioContext.Get<Configure<TOrigin>>();
                Configure<TOrigin> converted = o => additionalConfig((IDbContext)o);
                converted += origin;
                return converted;
            }
            
            var contextName = scenarioContext.Get<string>(CurrentContextName);
            if (contextName.ToUpper() == Configuration.DbContextType.Blue.Humanize().ToUpper())
            {
                scenarioContext.Configure(Convert<ZooContextBlue>(configure));
                return;
            }
            
            scenarioContext.Configure(Convert<ZooContextGreen>(configure));
        }
    }
}