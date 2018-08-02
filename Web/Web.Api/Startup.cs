using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Web.Api.Modules;

namespace WhiteRaven.Web.Api
{
    public class Startup
    {
        private const string SwaggerDocumentName = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            new TokenAuthenticationModule(Configuration).Load(services);
            new PasswordModule(Configuration).Load(services);
            new MemoryRepositoryModule(Configuration).Load(services);
            new DomainOperationsModule(Configuration).Load(services);
            new SwaggerModule(Configuration).Load(services);

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
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

            // Configure the swagger document
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/api/{documentName}";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Host = httpReq.Host.Value;
                    swaggerDoc.Schemes = new List<string> { "https" };
                });
            });

            // Configure the swagger UI
            app.UseSwaggerUI(c =>
            {
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.SwaggerEndpoint($"/api/{SwaggerDocumentName}", "White Raven API");
                c.DefaultModelsExpandDepth(0);
            });

            var repoInitializer = app.ApplicationServices.GetService<IContentInitializer>();

            var userRepo = app.ApplicationServices.GetService<IRepository<User>>();
            var noteRepo = app.ApplicationServices.GetService<IRepository<Note>>();
            var contributionRepo = app.ApplicationServices.GetService<IRepository<Contribution>>();

            repoInitializer.LoadContent("Mock/Users.json", userRepo);
            repoInitializer.LoadContent("Mock/Notes.json", noteRepo);
            repoInitializer.LoadContent("Mock/Contributions.json", contributionRepo);
        }
    }
}