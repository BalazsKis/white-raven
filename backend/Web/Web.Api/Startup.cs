using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WhiteRaven.Shared.DependencyInjection;
using WhiteRaven.Shared.Library.Configuration;
using WhiteRaven.Shared.Library.Extensions;

namespace WhiteRaven.Web.Api
{
    /// <summary>
    /// Used by the web host to start the application
    /// </summary>
    public class Startup
    {
        private readonly List<ModuleBase> _modules;


        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            _modules =
                Assembly
                    .GetEntryAssembly()
                    .GetReferencedAssemblies()
                    .Add(Assembly.GetEntryAssembly().GetName())
                    .Select(Assembly.Load)
                    .SelectMany(a => a.DefinedTypes)
                    .Where(type => FilterModulesToLoad(type, configuration.GetValue<Environment>("Environment")))
                    .Select(m => System.Activator.CreateInstance(m, configuration) as ModuleBase)
                    .ToList();
        }


        /// <summary>
        /// Adds services to the container (called by the runtime)
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Call the loading method of each module
            _modules.ForEach(m => m.Load(services));

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        /// <summary>
        /// Configures the HTTP request pipeline (called by the runtime)
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var isDev = env.IsDevelopment();

            if (!isDev)
            {
                app.UseHsts();
            }

            // Enable static file serving for the index page and its contents
            app.UseStaticFiles();

            // Redirect all requests to HTTPS
            app.UseHttpsRedirection();

            // Use and require authentication
            app.UseAuthentication();

            // Allow cross-domain requests
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            // Common exception catching for all controllers
            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Use attribute-based routing (homepage route defined here, so it won't be part of the swagger document)
            app.UseMvc(routes => { routes.MapRoute("", "{controller=Home}/{action=Index}"); });

            // Call the configuration method of each module
            _modules.ForEach(m => m.Configure(app));
        }


        private static bool FilterModulesToLoad(System.Type type, Environment currentEnvironment)
        {
            if (!typeof(ModuleBase).IsAssignableFrom(type))
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            var attribute = type.GetCustomAttribute<ForEnvironmentAttribute>();
            return attribute != null && attribute.IsForEnvironment(currentEnvironment);
        }
    }
}