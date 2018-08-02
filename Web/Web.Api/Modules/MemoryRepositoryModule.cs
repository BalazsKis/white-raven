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
    public class MemoryRepositoryModule : ModuleBase
    {
        public MemoryRepositoryModule(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Load(IServiceCollection serviceCollection)
        {
            // Register repository keys:
            serviceCollection
                .AddSingleton<IKeyFor<User>, UserKey>()
                .AddSingleton<IKeyFor<Note>, NoteKey>()
                .AddSingleton<IKeyFor<Contribution>, ContributionKey>();

            // Register repositories:
            serviceCollection
                .AddTransient<IRepository<User>, Repository<User>>()
                .AddTransient<IRepository<Note>, Repository<Note>>()
                .AddTransient<IRepository<Contribution>, Repository<Contribution>>();
            
            // Register content initializer:
            serviceCollection.AddSingleton<IContentInitializer, JsonFileContentIntitializer>();
        }
    }
}