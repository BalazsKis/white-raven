using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WhiteRaven.Shared.DependencyInjection
{
    public abstract class ModuleBase
    {
        protected IConfiguration Configuration { get; }

        protected ModuleBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public abstract void Load(IServiceCollection serviceCollection);

        public virtual void Configure(bool isDevelopmentEnvironment, IApplicationBuilder app)
        {
        }
    }
}