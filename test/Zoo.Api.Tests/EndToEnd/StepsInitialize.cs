namespace Zoo.Api.Tests.EndToEnd
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;

    using Core;

    using Infrastructure.Entities.Parameters;
    using Infrastructure.Entities.Zoo;
    using Infrastructure.Store;

    using Microsoft.Extensions.DependencyInjection;

    using TechTalk.SpecFlow;

    internal delegate void Assertion(IServiceProvider provider);

    internal delegate void Configure<in TDbContext>(TDbContext context);

    [Binding]
    public class StepsInitialize
    {
        private readonly ScenarioContext scenarioContext;

        public StepsInitialize(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var webFactory = this.scenarioContext.Get<WebFactory<Startup>>();
            this.scenarioContext.Get<Assertion>()(webFactory.Server.Services.CreateScope().ServiceProvider);
            webFactory.Dispose();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            this.scenarioContext.Set(string.Empty, ScenarioContextExtensions.CurrentContextName);
            this.scenarioContext.Set(new List<Claim>());
            this.scenarioContext.Set<Configure<IZooParametersContext>>(
                context =>
                    {
                        context.Set<Configuration>().Add(
                            new Configuration
                                {
                                    Key = "Writable",
                                    Value = this.scenarioContext.Get<string>(
                                        ScenarioContextExtensions.CurrentContextName)
                                });
                    });
            this.scenarioContext.Set<Configure<ZooContextBlue>>(context => { });
            this.scenarioContext.Set<Configure<ZooContextGreen>>(context => { });

            this.scenarioContext.Set<Configure<IServiceCollection>>(services => { });
            this.scenarioContext.Set<Assertion>(context => { });
            this.scenarioContext.Set(
                new WebFactory<Startup>(
                    this.scenarioContext.Get<Configure<IServiceCollection>>,
                    (serviceCollection, configure) =>
                        {
                            var zooParametersContextType = typeof(IZooParametersContext);
                            var zooBlueContextType = typeof(ZooContextBlue);
                            var zooGreenContextType = typeof(ZooContextGreen);
                            serviceCollection
                                .AddDbContextPool<IZooParametersContext, ZooParametersContext>(
                                    configure(zooParametersContextType.Name))
                                .AddDbContextPool<ZooContextBlue>(configure(zooBlueContextType.Name))
                                .AddDbContextPool<ZooContextGreen>(configure(zooGreenContextType.Name));

                            var dbContextConfigurations = new Dictionary<Type, Configure<IDbContext>>
                                                              {
                                                                  {
                                                                      zooParametersContextType,
                                                                      this.ToIDbContext<IZooParametersContext>()
                                                                  },
                                                                  {
                                                                      zooBlueContextType,
                                                                      this.ToIDbContext<ZooContextBlue>()
                                                                  },
                                                                  {
                                                                      zooGreenContextType,
                                                                      this.ToIDbContext<ZooContextGreen>()
                                                                  }
                                                              };
                            return dbContextConfigurations;
                        }));
        }

        private Configure<IDbContext> ToIDbContext<T>()
        {
            var configure = this.scenarioContext.Get<Configure<T>>();
            return o => configure((T)o);
        }
    }
}