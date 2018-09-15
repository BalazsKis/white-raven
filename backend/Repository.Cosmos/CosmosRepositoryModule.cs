using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Shared.DependencyInjection;
using WhiteRaven.Shared.Library.Configuration;

namespace WhiteRaven.Repository.Cosmos
{
    /// <summary>
    /// Contains registrations and configurations for the Azure CosmosDB based repository
    /// </summary>
    /// <seealso cref="ModuleBase"/>
    [ForEnvironment(Environment.Development, Environment.Staging, Environment.Production)]
    public class CosmosRepositoryModule : ModuleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosRepositoryModule"/> class.
        /// </summary>
        public CosmosRepositoryModule(IConfiguration configuration) : base(configuration)
        {
        }


        /// <summary>
        /// Registers repositories
        /// </summary>
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IRepository<User>, UserRepository>();
            //.AddSingleton<IRepository<Note>, NoteRepository>()
            //.AddSingleton<IRepository<Contribution>, ContributionRepository>();
        }
    }
}
