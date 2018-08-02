using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WhiteRaven.Shared.DependencyInjection;

namespace WhiteRaven.Web.Api.Modules
{
    /// <summary>
    /// Contains registrations and configurations for the Swagger documentation
    /// </summary>
    /// <seealso cref="ModuleBase"/>
    public class SwaggerModule : ModuleBase
    {
        private readonly string _swaggerName;


        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerModule"/> class.
        /// </summary>
        public SwaggerModule(IConfiguration configuration) : base(configuration)
        {
            _swaggerName = Configuration["Swagger:ApiVersion"];
        }


        /// <summary>
        /// Registers the swagger generation
        /// </summary>
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(_swaggerName, new Info
                    {
                        Title = "White Raven API",
                        Version = _swaggerName
                    });

                    c.DescribeAllEnumsAsStrings();

                    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
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
        public override void Configure(bool isDevelopmentEnvironment, IApplicationBuilder app)
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
                c.SwaggerEndpoint($"/api/{_swaggerName}", "White Raven API");
                c.DefaultModelsExpandDepth(0);
            });
        }
    }
}