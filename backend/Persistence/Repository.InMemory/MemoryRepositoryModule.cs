using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.InMemory.Keys;
using WhiteRaven.Shared.DependencyInjection;
using WhiteRaven.Shared.Library.Configuration;

namespace WhiteRaven.Repository.InMemory
{
    /// <summary>
    /// Contains registrations and configurations for the in-memory repository
    /// </summary>
    /// <seealso cref="ModuleBase"/>
    [ForEnvironment(Environment.Development)]
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
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<INoteRepository, NoteRepository>()
                .AddSingleton<IContributionRepository, ContributionRepository>();

            serviceCollection.AddSingleton<IContentInitializer, JsonFileContentInitializer>();
        }

        /// <summary>
        /// Initialize the repository with content from the mock json files
        /// </summary>
        public override void Configure(IApplicationBuilder app)
        {
            var repoInitializer = app.ApplicationServices.GetService<IContentInitializer>();

            var userRepo = app.ApplicationServices.GetService<IUserRepository>();
            var noteRepo = app.ApplicationServices.GetService<INoteRepository>();
            var contributionRepo = app.ApplicationServices.GetService<IContributionRepository>();

            repoInitializer.LoadContent("Mock/Users.json", userRepo);
            repoInitializer.LoadContent("Mock/Notes.json", noteRepo);
            repoInitializer.LoadContent("Mock/Contributions.json", contributionRepo);
        }
    }
}