using Microsoft.Extensions.DependencyInjection;
using SFSimulator.Core;
using System;
using System.Linq;

namespace SFSimulator.Tests
{
    public static class DepedencyProvider
    {
        private static IServiceProvider Provider { get; set; } = GetProvider();
        public static T GetRequiredService<T>() where T : notnull
        {
            return Provider.GetRequiredService<T>();
        }
        private static IServiceProvider GetProvider()
        {
            var services = new ServiceCollection();

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

            return services.BuildServiceProvider();
        }
    }
}