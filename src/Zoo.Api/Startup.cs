namespace Zoo.Api
{
    using System.Security.Claims;

    using Application;

    using Controllers;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    using Routing;

    using Swagger;

    using Swashbuckle.AspNetCore.SwaggerGen;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                    {
                        options.RoutePrefix = string.Empty;
                        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddAuthentication(
                        options =>
                            {
                                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                            })
                    .AddJwtBearer(
                        options =>
                            {
                                this.Configuration.GetSection("OidcOptions").Bind(options);
                                options.RequireHttpsMetadata = false;
                                options.TokenValidationParameters = new TokenValidationParameters
                                                                        {
                                                                            RoleClaimType = ClaimTypes.Role,
                                                                            ValidIssuer = options.Authority,
                                                                            ValidateAudience = true,
                                                                            ValidateIssuer = true,
                                                                            ValidateLifetime = true,
                                                                            RequireSignedTokens = true,
                                                                            NameClaimType = ClaimTypes.NameIdentifier
                                                                        };
                            })
                    .Services
                    .AddControllers()
                    .Services
                    .AddMvc(
                        options =>
                            {
                                options.Conventions.Add(
                                    new GenericControllerRouteConvention());
                            })
                    .ConfigureApplicationPartManager(
                        m =>
                            {
                                m.FeatureProviders.Add(
                                    new GenericControllerFeatureProvider(typeof(AnimalsController<,,>))
                                );
                            })
                    .Services
                    .AddApiVersioning(
                        config => { config.ReportApiVersions = true; })
                    .AddVersionedApiExplorer(
                        options =>
                            {
                                options.GroupNameFormat = "'v'VVV";
                                options.SubstituteApiVersionInUrl = true;
                            })
                    .AddApplication(
                        (builder, connectionString) =>
                            {
                                builder.UseSqlServer(
                                    this.Configuration.GetConnectionString(connectionString),
                                    options => options.EnableRetryOnFailure());
                            },
                        this
                            .Configuration
                            .GetSection)
                    .AddSwaggerGen();
        }
    }
}
