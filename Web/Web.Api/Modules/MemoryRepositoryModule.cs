using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.InMemory;
using WhiteRaven.Shared.DependencyInjection;
using WhiteRaven.Web.Api.Mock;

namespace WhiteRaven.Web.Api.Modules
{
    /// <summary>
    /// Contains registrations and configurations for the in-memory repository
    /// </summary>
    /// <seealso cref="ModuleBase"/>
    public class MemoryRepositoryModule : ModuleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryRepositoryModule"/> class.
        /// </summary>
        public MemoryRepositoryModule(IConfiguration configuration) : base(configuration)
        {
        }


        /// <summary>
        /// Registers repository keys, repositories and the content initializer
        /// </summary>
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IKeyFor<User>, UserKey>()
                .AddSingleton<IKeyFor<Note>, NoteKey>()
                .AddSingleton<IKeyFor<Contribution>, ContributionKey>();

            serviceCollection
                .AddTransient<IRepository<User>, Repository<User>>()
                .AddTransient<IRepository<Note>, Repository<Note>>()
                .AddTransient<IRepository<Contribution>, Repository<Contribution>>();

            serviceCollection.AddSingleton<IContentInitializer, JsonFileContentIntitializer>();
        }

        /// <summary>
        /// Initialize the repository with content from the mock json files
        /// </summary>
        public override void Configure(bool isDevelopmentEnvironment, IApplicationBuilder app)
        {
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