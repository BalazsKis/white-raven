using Microsoft.Extensions.DependencyInjection;

namespace WhiteRaven.Shared.DependencyInjection
{
    public interface IModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}