# MudBlazor ThemeManager Saver - Theme Saving Guide

## Overview

MudBlazor ThemeManager Saver is a powerful component that allows you to design, customize, save, and manage themes for MudBlazor applications. This enhanced version focuses on theme saving and persistence functionality.

## Current Theme Management Features

The `MudThemeManager` component provides comprehensive theme customization through the `ThemeManagerTheme` model, which includes:

### Theme Properties
- **Theme Colors**: Primary, Secondary, Tertiary, Info, Success, Warning, Error, Dark, Surface
- **Component Colors**: AppBar, Drawer, Background, Text colors
- **Typography**: Font family selection (Roboto, Montserrat, Ubuntu, Segoe UI)
- **Layout Settings**: Border radius, elevations, drawer clip modes
- **Dark/Light Mode**: Full palette support for both modes

### Current Implementation

The theme state is managed through the `UpdatePalette` method which:

```csharp
public Task UpdatePalette(ThemeUpdatedValue value)
{
    // Updates both theme palette and local palette objects
    // Supports both light and dark mode palettes
    // Triggers theme change events
}
```

## Theme Saving Implementation Guide

### 1. JSON Serialization

To save themes, you can serialize the `ThemeManagerTheme` object:

```csharp
// Save theme to JSON
public string SaveThemeToJson(ThemeManagerTheme theme)
{
    var options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    return JsonSerializer.Serialize(theme, options);
}

// Load theme from JSON
public ThemeManagerTheme LoadThemeFromJson(string json)
{
    var options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    return JsonSerializer.Deserialize<ThemeManagerTheme>(json, options) ?? new ThemeManagerTheme();
}
```

### 2. Local Storage Integration

For browser-based persistence:

```csharp
@inject IJSRuntime JSRuntime

// Save to localStorage
public async Task SaveThemeToLocalStorage(ThemeManagerTheme theme)
{
    var json = SaveThemeToJson(theme);
    await JSRuntime.InvokeVoidAsync("localStorage.setItem", "mudblazor-theme", json);
}

// Load from localStorage
public async Task<ThemeManagerTheme> LoadThemeFromLocalStorage()
{
    var json = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "mudblazor-theme");
    return string.IsNullOrEmpty(json) ? new ThemeManagerTheme() : LoadThemeFromJson(json);
}
```

### 3. File System Persistence

For server-side or desktop applications:

```csharp
// Save theme to file
public async Task SaveThemeToFile(ThemeManagerTheme theme, string filePath)
{
    var json = SaveThemeToJson(theme);
    await File.WriteAllTextAsync(filePath, json);
}

// Load theme from file
public async Task<ThemeManagerTheme> LoadThemeFromFile(string filePath)
{
    if (!File.Exists(filePath))
        return new ThemeManagerTheme();
    
    var json = await File.ReadAllTextAsync(filePath);
    return LoadThemeFromJson(json);
}
```

### 4. Integration with MainLayout

Update your MainLayout:

```razor
@inject IJSRuntime JSRuntime

<MudThemeProvider @ref="_mudThemeProvider" Theme="_themeManager.Theme" @bind-IsDarkMode="@_isDarkMode" />

<MudThemeManager 
    Open="_themeManagerOpen" 
    OpenChanged="OpenThemeManager" 
    Theme="_themeManager" 
    ThemeChanged="UpdateAndSaveTheme" 
    IsDarkMode="@_isDarkMode" />

@code {
    private ThemeManagerTheme _themeManager = new();
    private bool _themeManagerOpen;
    private bool _isDarkMode;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadSavedTheme();
            _isDarkMode = await _mudThemeProvider.GetSystemPreference();
            StateHasChanged();
        }
    }

    private async Task UpdateAndSaveTheme(ThemeManagerTheme theme)
    {
        _themeManager = theme;
        await SaveThemeToLocalStorage(theme);
        StateHasChanged();
    }

    private async Task LoadSavedTheme()
    {
        try
        {
            var savedTheme = await LoadThemeFromLocalStorage();
            if (savedTheme != null)
            {
                _themeManager = savedTheme;
            }
        }
        catch (Exception ex)
        {
            // Handle loading errors
            Console.WriteLine($"Error loading saved theme: {ex.Message}");
        }
    }
}
```

## Theme Preset System

You can create a preset system for commonly used themes:

```csharp
public static class ThemePresets
{
    public static ThemeManagerTheme DefaultLight => new()
    {
        Theme = new MudTheme(),
        FontFamily = "Roboto",
        DefaultBorderRadius = 4,
        DefaultElevation = 1,
        AppBarElevation = 25,
        DrawerElevation = 2,
        DrawerClipMode = DrawerClipMode.Never
    };

    public static ThemeManagerTheme MaterialDark => new()
    {
        // Configure dark theme preset
    };

    public static Dictionary<string, ThemeManagerTheme> GetAllPresets()
    {
        return new Dictionary<string, ThemeManagerTheme>
        {
            { "Default Light", DefaultLight },
            { "Material Dark", MaterialDark }
        };
    }
}
```

## Advanced Features

### Theme Validation

```csharp
public bool ValidateTheme(ThemeManagerTheme theme)
{
    return theme != null && 
           theme.Theme != null && 
           !string.IsNullOrEmpty(theme.FontFamily) &&
           theme.DefaultBorderRadius >= 0 &&
           theme.DefaultElevation >= 0;
}
```

### Theme Comparison

```csharp
public bool AreThemesEqual(ThemeManagerTheme theme1, ThemeManagerTheme theme2)
{
    var json1 = SaveThemeToJson(theme1);
    var json2 = SaveThemeToJson(theme2);
    return json1 == json2;
}
```

## Best Practices

1. **Error Handling**: Always wrap save/load operations in try-catch blocks
2. **Validation**: Validate theme data before saving
3. **Versioning**: Consider adding version information to saved themes
4. **Backup**: Implement backup mechanisms for important themes
5. **Performance**: Debounce save operations to avoid excessive writes

## Example Theme JSON Structure

```json
{
  "theme": {
    "paletteLight": {
      "primary": "#1976d2",
      "secondary": "#dc004e",
      "background": "#ffffff",
      // ... other palette colors
    },
    "paletteDark": {
      "primary": "#2196f3",
      "secondary": "#f48fb1",
      "background": "#121212",
      // ... other palette colors
    }
  },
  "fontFamily": "Roboto",
  "defaultBorderRadius": 4,
  "defaultElevation": 1,
  "appBarElevation": 25,
  "drawerElevation": 2,
  "drawerClipMode": "Never",
  "rtl": false
}
```

## Component Structure

### Core Components

- **MudThemeManager**: Main theme management component
- **MudThemeManagerButton**: Button component for theme manager
- **MudThemeManagerColorItem**: Individual color picker component

### Models

- **ThemeManagerTheme**: Main theme model containing all theme properties
- **ThemePaletteColor**: Individual color representation
- **ThemeUpdatedValue**: Value object for theme updates

### Extensions

- **Extension**: Utility methods for theme operations
- **PaletteSerializerContext**: JSON serialization context for palettes
- **ThemeSerializerContext**: JSON serialization context for themes

## Getting Started

1. Add the MudBlazor ThemeManager Saver package to your project
2. Include the component in your layout
3. Implement saving/loading functionality as shown in the examples above
4. Customize the theme manager appearance using the provided CSS classes

## Contributing

For more information on contributing to this project, see [CONTRIBUTING.md](../../CONTRIBUTING.md).

## License

This project is licensed under the MIT License - see the [LICENSE](../../LICENSE) file for details.
