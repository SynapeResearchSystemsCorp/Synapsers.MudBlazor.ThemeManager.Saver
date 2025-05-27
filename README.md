# Synapsers MudBlazor ThemeManager Saver

<p align="center">
  <img src="src/Synapsers.MudBlazor.ThemeManager.Saver/Synapsers.MudBlazor.ThemeManager.Saver.webp" alt="Synapsers MudBlazor Theme Manager Saver" style="max-width: 100%; height: auto;">
</p>

<p align="center">
  <a href="https://github.com/SynapeResearchSystemsCorp/Synapsers.MudBlazor.ThemeManager.Saver/stargazers">
    <img src="https://img.shields.io/github/stars/SynapeResearchSystemsCorp/Synapsers.MudBlazor.ThemeManager.Saver" alt="GitHub Repo stars">
  </a>
  <a href="https://github.com/SynapeResearchSystemsCorp/Synapsers.MudBlazor.ThemeManager.Saver/commits/main">
    <img src="https://img.shields.io/github/last-commit/SynapeResearchSystemsCorp/Synapsers.MudBlazor.ThemeManager.Saver" alt="GitHub last commit">
  </a>
  <a href="https://github.com/SynapeResearchSystemsCorp/Synapsers.MudBlazor.ThemeManager.Saver/graphs/contributors">
    <img src="https://img.shields.io/github/contributors/SynapeResearchSystemsCorp/Synapsers.MudBlazor.ThemeManager.Saver" alt="Contributors">
  </a>
  <a href="https://www.nuget.org/packages/Synapsers.MudBlazor.ThemeManager.Saver/">
    <img src="https://img.shields.io/nuget/v/Synapsers.MudBlazor.ThemeManager.Saver.svg" alt="NuGet version">
  </a>
  <a href="https://www.nuget.org/packages/Synapsers.MudBlazor.ThemeManager.Saver/">
    <img src="https://img.shields.io/nuget/dt/Synapsers.MudBlazor.ThemeManager.Saver.svg" alt="NuGet downloads">
  </a>
</p>

> **Enhanced Fork**: This project is an enhanced fork of the original [MudBlazor ThemeManager](https://github.com/MudBlazor/ThemeManager) with additional theme saving and persistence capabilities.

## 📋 Overview

Synapsers MudBlazor ThemeManager Saver solves a critical gap in the original MudBlazor ThemeManager: **the ability to save and persist user themes**. While the original component allows creating and customizing themes, those changes are lost when the application restarts. This enhanced version provides robust persistence solutions.

### Main Purpose

The primary goal of this component is to allow users to:

- **Create beautiful MudBlazor themes** with an intuitive visual interface
- **Save themes** across browser sessions and application restarts
- **Persist theme preferences** using various storage methods (localStorage, file system, etc.)
- **Toggle between light and dark modes** with proper persistence
- **Apply themes instantly** to enhance user experience

## ✨ Features

- **Advanced Theme Management**
  - Full visual editor for theme colors and properties
  - Real-time preview of theme changes
  - Complete control over primary, secondary, tertiary colors
  - Typography customization with font family selection
  - Layout settings (border radius, elevations, drawer clip modes)
  
- **Comprehensive Persistence Framework**
  - Browser localStorage for client-side Blazor applications
  - File system storage for server-side Blazor applications
  - JSON file persistence in wwwroot folder
  - Extensible design to add custom storage providers
  
- **Production-Ready Implementation**
  - Error handling with graceful fallbacks
  - Theme validation to prevent invalid configurations
  - Theme comparison utilities
  - Support for theme versioning
  - Automatic and manual save options

## 🔧 Quick Start

### Installation

```bash
dotnet add package Synapsers.MudBlazor.ThemeManager.Saver
```

Or via the NuGet Package Manager:

```powershell
Install-Package Synapsers.MudBlazor.ThemeManager.Saver
```

### Basic Implementation (3 Simple Steps)

1. **Register services in Program.cs**

```csharp
// For browser-based applications
builder.Services.AddMudServices();

// For server-side applications with file persistence
builder.Services.AddMudServices();
builder.Services.AddScoped<ThemePersistenceService>();
```

2. **Add to your MainLayout.razor**

```razor
@using Synapsers.MudBlazor.ThemeManager.Saver
@inject IJSRuntime JSRuntime

<MudThemeProvider @ref="_mudThemeProvider" Theme="_themeManager.Theme" @bind-IsDarkMode="@_isDarkMode" />
<MudThemeManager 
    Open="_themeManagerOpen" 
    OpenChanged="OpenThemeManager" 
    Theme="_themeManager" 
    ThemeChanged="UpdateAndSaveTheme" 
    IsDarkMode="@_isDarkMode" />
```

3. **Implement theme persistence in your code-behind**

```csharp
private ThemeManagerTheme _themeManager = new();
private bool _themeManagerOpen;
private bool _isDarkMode;

// Save theme when changed
private async Task UpdateAndSaveTheme(ThemeManagerTheme theme)
{
    _themeManager = theme;
    await JSRuntime.InvokeVoidAsync("localStorage.setItem", "mudblazor-theme", 
        JsonSerializer.Serialize(theme));
    StateHasChanged();
}

// Load saved theme on startup
protected override async Task OnInitializedAsync()
{
    var json = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "mudblazor-theme");
    if (!string.IsNullOrEmpty(json))
    {
        _themeManager = JsonSerializer.Deserialize<ThemeManagerTheme>(json);
    }
}
```

## 📖 Documentation

For detailed documentation and examples:

- [Theme Saving Implementation Guide](src/Synapsers.MudBlazor.ThemeManager.Saver/README.md) - Comprehensive guide with code samples
- [API Reference](https://github.com/SynapeResearchSystemsCorp/Synapsers.MudBlazor.ThemeManager.Saver/wiki) (Wiki)
- [Example Projects](https://github.com/SynapeResearchSystemsCorp/Synapsers.MudBlazor.ThemeManager.Saver/tree/main/examples)

## ⚡ Key Differentiators

What makes this component different from the original MudBlazor ThemeManager:

1. **Persistence Framework**: The original component lacks any theme persistence capabilities. Our enhanced version provides a complete persistence framework.

2. **Multiple Storage Options**: Support for various storage mechanisms (localStorage, file system, JSON) allows implementation in any Blazor hosting model.

3. **Dark Mode Persistence**: Remembers the user's dark/light mode preference across sessions.

4. **Production Ready**: Includes error handling, validation, and fallback mechanisms needed for real-world applications.

5. **Comprehensive Documentation**: Detailed implementation guides and examples make integration straightforward.

6. **Theme Presets**: System for managing common theme configurations and applying them instantly.

## 🧩 Persistence Methods

### Browser localStorage

```csharp
// Save theme
public async Task SaveThemeToLocalStorage(ThemeManagerTheme theme)
{
    var json = JsonSerializer.Serialize(theme, new JsonSerializerOptions { WriteIndented = true });
    await JSRuntime.InvokeVoidAsync("localStorage.setItem", "mudblazor-theme", json);
}

// Load theme
public async Task<ThemeManagerTheme> LoadThemeFromLocalStorage()
{
    var json = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "mudblazor-theme");
    return string.IsNullOrEmpty(json) 
        ? new ThemeManagerTheme() 
        : JsonSerializer.Deserialize<ThemeManagerTheme>(json);
}
```

### File System (Server-side)

```csharp
// Inject the ThemePersistenceService in your component
@inject ThemePersistenceService ThemePersistenceService

// Save theme
await ThemePersistenceService.SaveThemeAsync(_themeManager, _isDarkMode);

// Load theme
var persistedTheme = await ThemePersistenceService.LoadThemeAsync();
if (persistedTheme != null)
{
    _themeManager = persistedTheme.ThemeManagerTheme;
    _isDarkMode = persistedTheme.IsDarkMode;
}
```

## 📦 Project Structure

- **Components/**
  - `MudThemeManager.razor` - Main theme management component
  - `MudThemeManagerButton.razor` - Button component for theme manager
  - `MudThemeManagerColorItem.razor` - Individual color picker component

- **Models/**
  - `ThemeManagerTheme.cs` - Main theme model
  - `ThemePaletteColor.cs` - Color representation
  - `ThemeUpdatedValue.cs` - Value object for theme updates

- **Services/**
  - `ThemePersistenceService.cs` - Service for theme persistence
  - `ThemeStateService.cs` - Service for theme state management

- **Extensions/**
  - Utility methods and serialization contexts

## 👥 Contributing

Contributions are welcome! Please see our [Contributing Guide](CONTRIBUTING.md) for details on how to submit pull requests, report issues, and suggest improvements.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- [MudBlazor](https://mudblazor.com/) team for the original ThemeManager
- All contributors who have helped improve this component

---

<p align="center">
  <sub>Built with ❤️ by <a href="https://github.com/SynapeResearchSystemsCorp">Synapser Research Systems</a></sub>
</p>
