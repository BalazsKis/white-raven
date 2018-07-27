using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations;
using WhiteRaven.Domain.Operations.Interfaces;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.InMemory;
using WhiteRaven.Shared.Basics.Cryptography;
using WhiteRaven.Web.Api.Mock;

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
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services
                .AddSingleton<IPasswordGuard, PasswordGuard>()
                .AddSingleton<IContentInitializer, JsonFileContentIntitializer>()
                .AddSingleton<IKeyFor<User>, UserKey>()
                .AddSingleton<IKeyFor<Note>, NoteKey>()
                .AddSingleton<IKeyFor<Contribution>, ContributionKey>()
                .AddTransient<IRepository<User>, Repository<User>>()
                .AddTransient<IRepository<Note>, Repository<Note>>()
                .AddTransient<IRepository<Contribution>, Repository<Contribution>>()
                .AddSingleton<IUserOperations, UserOperations>()
                .AddSingleton<IContributionOperations, ContributionOperations>()
                .AddSingleton<INoteOperations, NoteOperations>()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc(SwaggerDocumentName, new Info
                    {
                        Title = "White Raven API",
                        Version = SwaggerDocumentName
                    });

                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });

                    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                        { "Bearer", Enumerable.Empty<string>() },
                    });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var repoInitializer = app.ApplicationServices.GetService<IContentInitializer>();

                var userRepo = app.ApplicationServices.GetService<IRepository<User>>();
                var noteRepo = app.ApplicationServices.GetService<IRepository<Note>>();
                var contributionRepo = app.ApplicationServices.GetService<IRepository<Contribution>>();

                repoInitializer.LoadContent("Mock/Users.json", userRepo);
                repoInitializer.LoadContent("Mock/Notes.json", noteRepo);
                repoInitializer.LoadContent("Mock/Contributions.json", contributionRepo);
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
                    swaggerDoc.Schemes = new List<string>{ "https" };
                });
            });

            // Configure the swagger UI
            app.UseSwaggerUI(c =>
            {
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.SwaggerEndpoint($"/api/{SwaggerDocumentName}", "White Raven API");
                c.DefaultModelsExpandDepth(0);
            });


        }
    }
}