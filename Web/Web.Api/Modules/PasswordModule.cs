using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhiteRaven.Shared.Cryptography;
using WhiteRaven.Shared.DependencyInjection;

namespace WhiteRaven.Web.Api.Modules
{
    public class PasswordModule : ModuleBase
    {
        public PasswordModule(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Load(IServiceCollection serviceCollection)
        {
            // Register password guard:
            serviceCollection.AddSingleton<IPasswordGuard, PasswordGuard>();
        }
    }
}