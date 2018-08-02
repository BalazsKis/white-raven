using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhiteRaven.Domain.Operations.Validation;
using WhiteRaven.Shared.DependencyInjection;

namespace WhiteRaven.Domain.Operations
{
    /// <summary>
    /// Contains registrations and configurations for the domain operations
    /// </summary>
    /// <seealso cref="ModuleBase"/>
    public class DomainOperationsModule : ModuleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainOperationsModule"/> class.
        /// </summary>
        public DomainOperationsModule(IConfiguration configuration) : base(configuration)
        {
        }


        /// <summary>
        /// Registers the domain model validators and the domain operations
        /// </summary>
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IUserValidator, UserValidator>()
                .AddSingleton<INoteValidator, NoteValidator>()
                .AddSingleton<IContributionValidator, ContributionValidator>();

            serviceCollection
                .AddSingleton<IUserOperations, UserOperations>()
                .AddSingleton<INoteOperations, NoteOperations>()
                .AddSingleton<IContributionOperations, ContributionOperations>();
        }
    }
}