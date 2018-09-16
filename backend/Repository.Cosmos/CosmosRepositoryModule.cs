using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.Cosmos.Configurations;
using WhiteRaven.Repository.Cosmos.Keys;
using WhiteRaven.Shared.DependencyInjection;
using WhiteRaven.Shared.Library.Configuration;

namespace WhiteRaven.Repository.Cosmos
{
    /// <summary>
    /// Contains registrations and configurations for the Azure CosmosDB based repository
    /// </summary>
    /// <seealso cref="ModuleBase"/>
    [ForEnvironment(Environment.Staging, Environment.Production)]
    public class CosmosRepositoryModule : ModuleBase
    {
        private readonly DbConnectionParameters _dbConnectionParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosRepositoryModule"/> class.
        /// </summary>
        public CosmosRepositoryModule(IConfiguration configuration) : base(configuration)
        {
            _dbConnectionParams = configuration?.GetSection("CosmosDbParams")?.Get<DbConnectionParameters>();
        }


        /// <summary>
        /// Registers repositories and entity key providers
        /// </summary>
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton(_dbConnectionParams);

            serviceCollection
                .AddSingleton<IKeyFor<User>, UserKey>()
                .AddSingleton<IKeyFor<Note>, NoteKey>()
                .AddSingleton<IKeyFor<Contribution>, ContributionKey>();

            serviceCollection
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<INoteRepository, NoteRepository>()
                .AddSingleton<IContributionRepository, ContributionRepository>();
        }
    }
}