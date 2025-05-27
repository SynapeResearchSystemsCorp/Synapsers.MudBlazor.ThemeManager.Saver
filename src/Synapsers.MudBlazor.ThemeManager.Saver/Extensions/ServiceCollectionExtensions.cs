using Microsoft.Extensions.DependencyInjection;
using Synapsers.MudBlazor.ThemeManager.Saver.Services;

namespace Synapsers.MudBlazor.ThemeManager.Saver.Extensions;

public static class ServiceCollectionExtensions
{    /// <summary>
    /// Adds theme persistence services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddThemePersistence(this IServiceCollection services)
    {
        services.AddSingleton<ThemePersistenceService>();
        services.AddSingleton<ThemeStateService>();
        return services;
    }
}
