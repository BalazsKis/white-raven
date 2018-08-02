using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using WhiteRaven.Domain.Operations;
using WhiteRaven.Shared.DependencyInjection;
using WhiteRaven.Web.Api.Modules;

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
            // ToDo: Get from assemblies with reflection
            _modules = new List<ModuleBase>
            {
                new TokenAuthenticationModule(configuration),
                new PasswordModule(configuration),
                new MemoryRepositoryModule(configuration),
                new DomainOperationsModule(configuration),
                new SwaggerModule(configuration)
            };
        }


        /// <summary>
        /// Adds services to the container (called by the runtime)
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Run the configuration method of each module
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

            // Common exception catching for all controllers
            app.UseMiddleware<ErrorHandlingMiddleware>();

            // Use attribute-based routing (homepage route defined here, so it won't be part of the swagger document)
            app.UseMvc(routes => { routes.MapRoute("", "{controller=Home}/{action=Index}"); });

            // Run the configuration method of each module
            _modules.ForEach(m => m.Configure(isDev, app));
        }
    }
}