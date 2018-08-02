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
    public class SwaggerModule : ModuleBase
    {
        public SwaggerModule(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Load(IServiceCollection serviceCollection)
        {
            // Register swagger generation:
            serviceCollection
                .AddSwaggerGen(c =>
                {
                    var name = Configuration["Swagger:ApiVersion"];

                    c.SwaggerDoc(name, new Info
                    {
                        Title = "White Raven API",
                        Version = name
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
    }
}