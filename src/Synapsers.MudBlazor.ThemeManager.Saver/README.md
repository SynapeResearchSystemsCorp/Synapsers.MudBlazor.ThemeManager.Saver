# Synapsers MudBlazor ThemeManager Saver

> **Enhanced Fork**: This project is an enhanced fork of the original [MudBlazor ThemeManager](https://github.com/MudBlazor/ThemeManager) with robust theme saving and persistence capabilities for Blazor applications.

## Overview

**Synapsers MudBlazor ThemeManager Saver** is a comprehensive solution for designing, customizing, saving, and managing themes in MudBlazor-based applications. It addresses a key limitation of the original ThemeManager by providing a flexible, extensible persistence framework for user themes and preferences.

---

## ✨ Features

- **Visual Theme Editor**: Intuitive UI for customizing all MudBlazor theme properties (colors, typography, layout, etc.)
- **Persistence Framework**: Save and load themes using browser localStorage, server file system, or custom providers
- **Dark/Light Mode**: Full support for toggling and persisting dark/light mode
- **Theme Presets**: Easily manage and apply common theme configurations
- **Production-Ready**: Error handling, validation, versioning, and extensibility
- **Extensible**: Add your own storage providers or extend theme models

---

## Architecture & Core Components

- **MudThemeManager**: Main component for theme editing and preview
- **ThemeManagerTheme**: Model representing all theme properties (colors, typography, layout, etc.)
- **ThemePersistenceService**: Handles saving/loading themes (JSON, file, localStorage, etc.)
- **ThemeStateService**: Manages current theme state, initialization, and change notifications
- **Extensions**: Utility methods and serialization helpers

---

## How It Works

1. **User customizes theme** in the UI (colors, fonts, layout, etc.)
2. **ThemeManagerTheme** model is updated and previewed live
3. **ThemePersistenceService** saves the theme to the chosen storage (localStorage, file, etc.)
4. **ThemeStateService** loads and applies the theme on app startup, and notifies components of changes
5. **Dark/Light mode** and all preferences are persisted and restored automatically

---

## Usage & Integration

### 1. Register Services

```csharp
// In Program.cs
builder.Services.AddMudServices();
builder.Services.AddThemePersistence(); // Registers ThemePersistenceService and ThemeStateService
```

### 2. Add to Layout

```razor
@using Synapsers.MudBlazor.ThemeManager.Saver
@inject ThemeStateService ThemeStateService

<MudThemeProvider Theme="ThemeStateService.ThemeManager.Theme" @bind-IsDarkMode="ThemeStateService.IsDarkMode" />
<MudThemeManagerButton OnClick="@((e) => OpenThemeManager(true))" />
<MudThemeManager Open="_themeManagerOpen" OpenChanged="OpenThemeManager" Theme="ThemeStateService.ThemeManager" ThemeChanged="UpdateThemeAsync" IsDarkMode="ThemeStateService.IsDarkMode" />
```

### 3. Persisting Themes

- **Browser (localStorage):**
  - Uses JSInterop to save/load JSON theme data
- **Server (File System):**
  - Saves/loads JSON files in `wwwroot` or a configurable path
- **Custom Providers:**
  - Implement your own by extending `ThemePersistenceService`

### 4. Example: Save/Load Theme (localStorage)

```csharp
public async Task SaveThemeToLocalStorage(ThemeManagerTheme theme)
{
    var json = JsonSerializer.Serialize(theme, new JsonSerializerOptions { WriteIndented = true });
    await JSRuntime.InvokeVoidAsync("localStorage.setItem", "mudblazor-theme", json);
}

public async Task<ThemeManagerTheme> LoadThemeFromLocalStorage()
{
    var json = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "mudblazor-theme");
    return string.IsNullOrEmpty(json) ? new ThemeManagerTheme() : JsonSerializer.Deserialize<ThemeManagerTheme>(json);
}
```

### 5. Example: Save/Load Theme (File System)

```csharp
public async Task SaveThemeToFile(ThemeManagerTheme theme, string filePath)
{
    var json = JsonSerializer.Serialize(theme, new JsonSerializerOptions { WriteIndented = true });
    await File.WriteAllTextAsync(filePath, json);
}

public async Task<ThemeManagerTheme> LoadThemeFromFile(string filePath)
{
    if (!File.Exists(filePath)) return new ThemeManagerTheme();
    var json = await File.ReadAllTextAsync(filePath);
    return JsonSerializer.Deserialize<ThemeManagerTheme>(json) ?? new ThemeManagerTheme();
}
```

---

## Theme Presets

You can define and use theme presets for quick switching:

```csharp
public static class ThemePresets
{
    public static ThemeManagerTheme DefaultLight => new() { /* ... */ };
    public static ThemeManagerTheme MaterialDark => new() { /* ... */ };
    public static Dictionary<string, ThemeManagerTheme> GetAllPresets() => new() {
        { "Default Light", DefaultLight },
        { "Material Dark", MaterialDark }
    };
}
```

---

## Advanced Features

- **Validation**: Ensure theme data is valid before saving
- **Comparison**: Compare two themes for equality
- **Versioning**: Add version info to saved themes for migration
- **Automatic/Manual Save**: Save on every change or on demand

---

## Best Practices

1. Always validate theme data before saving
2. Use try-catch for all persistence operations
3. Register your persistence service in `Program.cs`
4. Use the `ThemeChanged` event for automatic saving
5. Consider file permissions and backup for server-side storage

---

## Example Theme JSON

```json
{
  "themeManagerTheme": {
    "theme": {
      "paletteLight": { "primary": "#1976d2", ... },
      "paletteDark": { "primary": "#2196f3", ... }
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

---

## Project Structure

- **Components/**
  - `MudThemeManager.razor` - Main theme management UI
  - `MudThemeManagerButton.razor` - Button to open the theme manager
  - `MudThemeManagerColorItem.razor` - Color picker for palette items
- **Models/**
  - `ThemeManagerTheme.cs` - Theme model
  - `ThemePaletteColor.cs` - Color representation
  - `ThemeUpdatedValue.cs` - Value object for updates
- **Services/**
  - `ThemePersistenceService.cs` - Theme persistence logic
  - `ThemeStateService.cs` - Theme state and notification
- **Extensions/**
  - Utility and serialization helpers

---

## Getting Started

1. Add the package: `dotnet add package Synapsers.MudBlazor.ThemeManager.Saver`
2. Register services in `Program.cs`
3. Add the components to your layout
4. Choose and configure your persistence method
5. Enjoy persistent, user-friendly theme management in your MudBlazor app!

---

## Contributing & Support

Contributions are welcome! See [CONTRIBUTING.md](../../CONTRIBUTING.md) for details.

---

## License

MIT License. See [LICENSE](../../LICENSE).

---

## Acknowledgments

- [MudBlazor](https://mudblazor.com/) for the original ThemeManager
- All contributors and users who helped improve this project




