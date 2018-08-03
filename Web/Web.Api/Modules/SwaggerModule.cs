using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WhiteRaven.Shared.DependencyInjection;
using WhiteRaven.Shared.Library.Configuration;
using WhiteRaven.Web.Api.ConfigurationObjects;

namespace WhiteRaven.Web.Api.Modules
{
    /// <summary>
    /// Contains registrations and configurations for the Swagger documentation
    /// </summary>
    /// <seealso cref="ModuleBase"/>
    [ForEnvironment(Environment.Development, Environment.Staging, Environment.Production)]
    public class SwaggerModule : ModuleBase
    {
        private readonly SwaggerParameters _swaggerParameters;


        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerModule"/> class.
        /// </summary>
        public SwaggerModule(IConfiguration configuration) : base(configuration)
        {
            _swaggerParameters = configuration.GetSection("Swagger").Get<SwaggerParameters>();
        }


        /// <summary>
        /// Registers the swagger generation
        /// </summary>
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(_swaggerParameters.ApiVersion, new Info
                    {
                        Title = _swaggerParameters.ApiName,
                        Version = _swaggerParameters.ApiVersion
                    });

                    c.DescribeAllEnumsAsStrings();

                    var xmlPath = Path.Combine(System.AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }

                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });

                    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer", Enumerable.Empty<string>()}
                    });

                    c.OperationFilter<ExamplesOperationFilter>();
                });
        }

        /// <summary>
        /// Configures the swagger document and the swagger UI
        /// </summary>
        public override void Configure(IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/api/{documentName}";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Host = httpReq.Host.Value;
                    swaggerDoc.Schemes = new List<string> { "https" };
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.SwaggerEndpoint($"/api/{_swaggerParameters.ApiVersion}", _swaggerParameters.ApiName);
                c.DefaultModelsExpandDepth(0);
            });
        }
    }
}