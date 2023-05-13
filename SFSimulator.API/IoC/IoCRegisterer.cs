using QuestSimulator.IoC;
using SFSimulatorAPI.Services;

namespace SFSimulatorAPI.IoC
{
    public static class IoCRegisterer
    {
        public static void RegisterQuestSimulator(this IServiceCollection services)
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
        public static void RegisterControllerServices(this IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();
        }
    }
}
