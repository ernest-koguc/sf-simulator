using SFSimulator.Core;

namespace SFSimulator.Frontend;

public static class DependencyRegisterer
{
    public static void RegisterSimulatorCore(this IServiceCollection services)
    {
        foreach (var type in TypesMapper.Types)
        {
            services.AddScoped(type.Interface, type.Implementation);
        }
    }
}
