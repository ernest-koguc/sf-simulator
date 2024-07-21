using SFSimulator.Core;

namespace SFSimulator.Frontend;

public static class DependencyRegisterer
{
    public static void RegisterSimulatorCore(this IServiceCollection services)
    {
        var typeMaps = TypesMapper.Types();
        foreach (var type in typeMaps.Where(x => x.TypeMapOption == TypeMapOption.None))
        {
            services.AddScoped(type.Interface, type.Implementation);
        }

        foreach (var type in typeMaps.Where(x => x.TypeMapOption == TypeMapOption.Singleton))
        {
            services.AddSingleton(type.Interface, type.Implementation);
        }

        foreach (var type in typeMaps.Where(x => x.TypeMapOption == TypeMapOption.Self))
        {
            services.AddScoped(type.Implementation);
        }
    }
}
