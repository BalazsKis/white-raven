using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhiteRaven.Domain.Operations.Validation;
using WhiteRaven.Shared.DependencyInjection;

namespace WhiteRaven.Domain.Operations
{
    public class DomainOperationsModule : ModuleBase
    {
        public DomainOperationsModule(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Load(IServiceCollection serviceCollection)
        {
            // Register validators:
            serviceCollection
                .AddSingleton<IUserValidator, UserValidator>()
                .AddSingleton<INoteValidator, NoteValidator>()
                .AddSingleton<IContributionValidator, ContributionValidator>();

            // Register domain operations:
            serviceCollection
                .AddSingleton<IUserOperations, UserOperations>()
                .AddSingleton<INoteOperations, NoteOperations>()
                .AddSingleton<IContributionOperations, ContributionOperations>();
        }
    }
}