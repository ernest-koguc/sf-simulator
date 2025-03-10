using Microsoft.Extensions.DependencyInjection;
using SFSimulator.Core;
using System;

namespace SFSimulator.Tests;

public static class DependencyProvider
{
    private static IServiceProvider Provider { get; set; } = GetProvider();
    public static T Get<T>() where T : notnull
    {
        using var scope = Provider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
    private static IServiceProvider GetProvider()
    {
        var services = new ServiceCollection();

        foreach (var type in TypesMapper.Types)
        {
            services.AddScoped(type.Interface, type.Implementation);
        }

        return services.BuildServiceProvider();
    }
}