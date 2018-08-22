using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhiteRaven.Shared.DependencyInjection;
using WhiteRaven.Shared.Library.Configuration;
using WhiteRaven.Shared.Library.Cryptography;

namespace WhiteRaven.Web.Api.Modules
{
    /// <summary>
    /// Contains registrations and configurations for the password manager
    /// </summary>
    /// <seealso cref="ModuleBase"/>
    [ForEnvironment(Environment.Development, Environment.Staging, Environment.Production)]
    public class PasswordModule : ModuleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordModule"/> class.
        /// </summary>
        public PasswordModule(IConfiguration configuration) : base(configuration)
        {
        }


        /// <summary>
        /// Registers the password guard
        /// </summary>
        public override void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPasswordGuard, PasswordGuard>();
        }
    }
}