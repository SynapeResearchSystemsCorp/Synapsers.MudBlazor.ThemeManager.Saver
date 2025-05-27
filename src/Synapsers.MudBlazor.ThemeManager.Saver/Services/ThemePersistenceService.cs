using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace Synapsers.MudBlazor.ThemeManager.Saver.Services;

public class ThemePersistenceService
{
    private readonly IHostEnvironment _hostEnvironment;
    private const string ThemeFileName = "theme.json";

    public ThemePersistenceService(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    private string GetThemeFilePath()
    {
        return Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", ThemeFileName);
    }

    public async Task SaveThemeAsync(ThemeManagerTheme themeManagerTheme, bool isDarkMode)
    {
        var themeData = new PersistedThemeData
        {
            ThemeManagerTheme = themeManagerTheme,
            IsDarkMode = isDarkMode
        };
        var jsonTheme = JsonSerializer.Serialize(themeData, new JsonSerializerOptions { WriteIndented = true });
        var filePath = GetThemeFilePath();
        await File.WriteAllTextAsync(filePath, jsonTheme);
    }

    public async Task<PersistedThemeData?> LoadThemeAsync()
    {
        var filePath = GetThemeFilePath();
        if (File.Exists(filePath))
        {
            var jsonTheme = await File.ReadAllTextAsync(filePath);
            if (!string.IsNullOrEmpty(jsonTheme))
            {
                try
                {
                    var themeData = JsonSerializer.Deserialize<PersistedThemeData>(jsonTheme);
                    return themeData;
                }
                catch (JsonException)
                {
                    // Handle deserialization error, e.g., corrupted data
                    // Optionally, log this error
                    return null;
                }
            }
        }
        return null;
    }
}

public class PersistedThemeData
{
    public ThemeManagerTheme ThemeManagerTheme { get; set; } = new();
    public bool IsDarkMode { get; set; }
}
