using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMvc();
        }
    }
}