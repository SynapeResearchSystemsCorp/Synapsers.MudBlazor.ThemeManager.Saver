# Synapser MudBlazor ThemeManager Saver - Theme Saving Guide


> **Enhanced Fork**: This project is an enhanced fork of the original [MudBlazor ThemeManager](https://github.com/MudBlazor/ThemeManager) with additional theme saving and persistence capabilities.

## Overview

Synapser MudBlazor ThemeManager Saver is a powerful component that allows you to design, customize, save, and manage themes for MudBlazor applications. This enhanced version focuses on theme saving and persistence functionality.

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

### 4. JSON File Persistence Service

For applications that need to persist themes as JSON files in the wwwroot folder:

```csharp
using Microsoft.AspNetCore.Hosting;
using Synapsers.MudBlazor.ThemeManager.Saver;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class ThemePersistenceService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private const string ThemeFileName = "theme.json";

    public ThemePersistenceService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    private string GetThemeFilePath()
    {
        return Path.Combine(_webHostEnvironment.WebRootPath, ThemeFileName);
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
```

Register the service in your `Program.cs`:

```csharp
builder.Services.AddScoped<ThemePersistenceService>();
```

### 5. Integration with MainLayout

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

### 6. MainLayout with JSON File Persistence Service

For applications using the JSON file persistence service:

```razor
@using YourApplication.Services
@inherits LayoutComponentBase
@inject IJSRuntime JSRuntime
@inject ThemePersistenceService ThemePersistenceService

<MudThemeProvider @ref="@_mudThemeProvider" Theme="_themeManager.Theme" @bind-IsDarkMode="@_isDarkMode" />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider /> 

<MudLayout>
    <MudAppBar Elevation="_themeManager.AppBarElevation">
        <MudIconButton Icon="@Icons.Material.Filled.Menu" Color="Color.Inherit" Edge="Edge.Start" OnClick="@((e) => DrawerToggle())" />
        <MudSpacer />
        <MudIconButton Icon="@Icons.Material.Filled.DarkMode" Color="Color.Inherit" OnClick="@((e) => DarkModeToggleAsync())" Edge="Edge.End" />
    </MudAppBar>
    <MudDrawer @bind-Open="_drawerOpen" ClipMode="_themeManager.DrawerClipMode" Elevation="_themeManager.DrawerElevation">
        <NavMenu />
    </MudDrawer>
    <MudMainContent>
        <MudContainer MaxWidth="MaxWidth.False" Class="mt-16 px-16">
            @Body
        </MudContainer>
    </MudMainContent>
    <MudThemeManagerButton OnClick="@((e) => OpenThemeManager(true))" />
    <MudThemeManager @key="_themeManager" Open="_themeManagerOpen" OpenChanged="OpenThemeManager" Theme="_themeManager" ThemeChanged="UpdateThemeAsync" IsDarkMode="@_isDarkMode" />
</MudLayout>

@code {
    private ThemeManagerTheme _themeManager = new();
    private MudThemeProvider _mudThemeProvider;

    private bool _isDarkMode;
    private bool _drawerOpen = true;
    private bool _themeManagerOpen;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void OpenThemeManager(bool value)
    {
        _themeManagerOpen = value;
    }

    private async Task DarkModeToggleAsync()
    {
        _isDarkMode = !_isDarkMode;
        await ThemePersistenceService.SaveThemeAsync(_themeManager, _isDarkMode);
        StateHasChanged();
    }

    private async Task UpdateThemeAsync(ThemeManagerTheme value)
    {
        _themeManager = value;
        await ThemePersistenceService.SaveThemeAsync(_themeManager, _isDarkMode);
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var persistedTheme = await ThemePersistenceService.LoadThemeAsync();
            if (persistedTheme != null)
            {
                _themeManager = persistedTheme.ThemeManagerTheme;
                _isDarkMode = persistedTheme.IsDarkMode;
            }
            else
            {
                _isDarkMode = await _mudThemeProvider.GetSystemPreference();
            }
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
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
6. **Service Registration**: Remember to register your persistence service in `Program.cs`
7. **File Permissions**: Ensure your application has write permissions to the wwwroot folder
8. **Automatic Persistence**: Use the `ThemeChanged` event to automatically save themes when modified

## Example Theme JSON Structure

Here's an example of how the persisted theme data looks when saved as JSON:

```json
{
  "themeManagerTheme": {
    "theme": {
      "paletteLight": {
        "primary": "#1976d2",
        "secondary": "#dc004e",
        "background": "#ffffff",
        "surface": "#ffffff",
        "appbarBackground": "#1976d2",
        "drawerBackground": "#ffffff"
        // ... other palette colors
      },
      "paletteDark": {
        "primary": "#2196f3",
        "secondary": "#f48fb1",
        "background": "#121212",
        "surface": "#1e1e1e",
        "appbarBackground": "#1976d2",
        "drawerBackground": "#1e1e1e"
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
  },
  "isDarkMode": false
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

### Quick Setup

1. **Add the Package**: Add the Synapser MudBlazor ThemeManager Saver package to your project
2. **Choose Persistence Method**: 
   - For browser storage: Use the localStorage approach
   - For server-side persistence: Use the JSON file persistence service
3. **Register Services**: Add your chosen persistence service to `Program.cs`
4. **Update MainLayout**: Implement the theme loading/saving logic in your layout
5. **Customize Appearance**: Use the provided CSS classes to customize the theme manager

### For JSON File Persistence

1. Register the service:
   ```csharp
   builder.Services.AddScoped<ThemePersistenceService>();
   ```

2. Include the component in your MainLayout as shown in the examples above

3. The theme will automatically:
   - Load on application startup
   - Save when theme changes are made
   - Save when dark/light mode is toggled

## Contributing

For more information on contributing to this project, see [CONTRIBUTING.md](../../CONTRIBUTING.md).
