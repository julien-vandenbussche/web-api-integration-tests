namespace Zoo.Infrastructure
{
    using System;
    using System.Linq;

    using Adapters;

    using Administration.AnimalsRegistrationAggregate;

    using AutoMapper;

    using AxaFrance.Extensions.ServiceModel;
    using AxaFrance.Extensions.ServiceModel.Settings;

    using BookService;

    using Contracts.Veterinary;

    using Entities.Parameters;
    using Entities.Zoo;

    using Humanizer;

    using Infirmary.VeterinaryAggregate;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Park.BooksAggregate;
    using Park.Common.Adapters;

    using Store;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder, string> dbContextConfiguration,
            Func<string, IConfigurationSection> getSection)
        {
            
            var bookServiceConfiguration = new ServiceConfiguration<BasicAuth>();
            getSection("ServicesOptions:BookService").Bind(bookServiceConfiguration);
            return services.AddAutoMapper(dbContextConfiguration.Method.DeclaringType.Assembly, typeof(ServiceCollectionExtensions).Assembly)
                           .AddScoped<IReader, Reader>()
                           .AddScoped<IWriter, Writer>()
                           .AddDbContextPool<ZooContextBlue>(builder => dbContextConfiguration(builder, "ZooContextBlue"))
                           .AddDbContextPool<ZooContextGreen>(builder => dbContextConfiguration(builder, "ZooContextGreen"))
                           .AddDbContextPool<IZooParametersContext, ZooParametersContext>(builder => dbContextConfiguration(builder, "ZooParametersContext"))
                           .AddScoped(GetDbContext)
                           .AddScoped<IRestrainedAnimalAdapter, RestrainedAnimalAdapter>()
                           .AddScoped<IAnimalsRegistrationAdapter, AnimalsRegistrationAdapter>()
                           .AddScoped<IVeterinaryAdapter, VeterinaryAdapter>()
                           .AddScoped<IBookAdapter, BookAdapter>()
                           .AddHttpClient("veterinary-list",
                               (provider, options) =>
                                   {
                                       var configuration = provider.GetRequiredService<IConfiguration>();
                                       configuration.Bind("veterinary-list", options);
                                   })
                           .AddTypedClient(Refit.RestService.For<IVeterinaryClient>)
                           .Services
                           .AddWcfClient<BookServiceChannel>(bookServiceConfiguration);
        }

        private static IDbContext GetDbContext(IServiceProvider serviceProvider)
        {
            using var zooParameters = serviceProvider.GetRequiredService<IZooParametersContext>();
            var configuration = zooParameters.Set<Configuration>();
            var value = configuration.Single(c => c.Key.ToUpper() == Configuration.WritableKey).Value;
            if (value.ToUpperInvariant() == Configuration.DbContextType.Blue.Humanize().ToUpper())
            {
                return serviceProvider.GetRequiredService<ZooContextBlue>();
            }
            
            return serviceProvider.GetRequiredService<ZooContextGreen>();
        }
    }
}