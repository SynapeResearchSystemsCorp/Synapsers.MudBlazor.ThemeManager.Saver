using Synapsers.MudBlazor.ThemeManager.Saver;

namespace Synapsers.MudBlazor.ThemeManager.Saver.Services;

/// <summary>
/// Service that manages the application's theme state and ensures theme loading happens only once
/// </summary>
public class ThemeStateService
{
    private readonly ThemePersistenceService _persistenceService;
    private bool _isInitialized = false;
    private ThemeManagerTheme _themeManager = new();
    private bool _isDarkMode = false;

    public ThemeStateService(ThemePersistenceService persistenceService)
    {
        _persistenceService = persistenceService;
    }

    /// <summary>
    /// Gets the current theme manager instance
    /// </summary>
    public ThemeManagerTheme ThemeManager => _themeManager;

    /// <summary>
    /// Gets or sets the dark mode state
    /// </summary>
    public bool IsDarkMode 
    { 
        get => _isDarkMode;
        set
        {
            _isDarkMode = value;
            OnThemeChanged?.Invoke();
        }
    }

    /// <summary>
    /// Gets whether the theme has been initialized
    /// </summary>
    public bool IsInitialized => _isInitialized;

    /// <summary>
    /// Event that fires when the theme changes
    /// </summary>
    public event Action? OnThemeChanged;

    /// <summary>
    /// Initializes the theme from persistence (should be called only once)
    /// </summary>
    public async Task InitializeAsync(bool? systemPreference = null)
    {
        if (_isInitialized)
            return;

        var persistedTheme = await _persistenceService.LoadThemeAsync();
        if (persistedTheme != null)
        {
            _themeManager = persistedTheme.ThemeManagerTheme;
            _isDarkMode = persistedTheme.IsDarkMode;
        }
        else if (systemPreference.HasValue)
        {
            _isDarkMode = systemPreference.Value;
        }

        _isInitialized = true;
        OnThemeChanged?.Invoke();
    }

    /// <summary>
    /// Updates the theme and persists it
    /// </summary>
    public async Task UpdateThemeAsync(ThemeManagerTheme theme)
    {
        _themeManager = theme;
        await _persistenceService.SaveThemeAsync(_themeManager, _isDarkMode);
        OnThemeChanged?.Invoke();
    }

    /// <summary>
    /// Toggles dark mode and persists the change
    /// </summary>
    public async Task ToggleDarkModeAsync()
    {
        _isDarkMode = !_isDarkMode;
        await _persistenceService.SaveThemeAsync(_themeManager, _isDarkMode);
        OnThemeChanged?.Invoke();
    }
}
