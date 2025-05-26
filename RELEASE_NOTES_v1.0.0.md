# 🎨 MudBlazor ThemeManager Saver v1.0.0

## 🚀 First Official Release

This is the **first official release** of **Synapsers MudBlazor ThemeManager Saver** - an enhanced fork of the original [MudBlazor ThemeManager](https://github.com/MudBlazor/ThemeManager) with powerful theme saving and persistence capabilities!

## ✨ What's Included

### 🎯 **Core Features**
- **🎨 Enhanced Theme Management**: All original ThemeManager features plus persistence
- **💾 JSON Serialization**: Save and load themes as JSON files using System.Text.Json
- **🔄 Automatic Persistence**: Themes save automatically when modified
- **🌓 Dark/Light Mode Support**: Full palette support for both light and dark modes
- **⚡ Multi-Target Framework**: Supports both .NET 8.0 and .NET 9.0

### 💾 **Persistence Capabilities**
- **🌐 Local Storage**: Browser-based persistence for client-side Blazor apps
- **📁 File System**: Server-side file storage for Blazor Server applications
- **📄 wwwroot JSON**: JSON files stored in wwwroot folder
- **🔧 Extensible**: Easy to add custom persistence providers

### 🛠️ **Technical Features**
- **🔒 Type Safety**: Strongly-typed theme models with `ThemeManagerTheme`
- **🛡️ Error Handling**: Robust error handling and validation
- **🚀 Performance**: Efficient JSON serialization with source generators
- **📱 Responsive**: Works seamlessly across all device sizes
- **♿ Accessibility**: Full accessibility support

## 📦 **Components Included**

| Component | Description |
|-----------|-------------|
| `MudThemeManager` | Main theme management component with persistence |
| `MudThemeManagerButton` | Quick access button for theme management |
| `MudThemeManagerColorItem` | Individual color picker component |

## 🔧 **Models & Extensions**

| Type | Description |
|------|-------------|
| `ThemeManagerTheme` | Complete theme configuration model |
| `ThemePaletteColor` | Individual color definition |
| `ThemeUpdatedValue` | Theme update event arguments |
| `PaletteSerializerContext` | JSON serialization context for palettes |
| `ThemeSerializerContext` | JSON serialization context for themes |

## 🎨 **Theme Customization Features**

### Color Management
- **Primary Colors**: Primary, Secondary, Tertiary
- **Status Colors**: Info, Success, Warning, Error
- **UI Colors**: Background, Surface, AppBar, Drawer
- **Text Colors**: Primary, Secondary, Disabled text
- **Dark Mode**: Complete dark palette support

### Typography & Layout
- **Font Families**: Roboto, Montserrat, Ubuntu, Segoe UI
- **Border Radius**: Customizable corner rounding
- **Elevations**: Material Design elevation system
- **Drawer Modes**: Responsive, Persistent, Mini variants

## 🚀 **Getting Started**

### Installation

```bash
dotnet add package Synapsers.MudBlazor.ThemeManager.Saver
```

### Quick Setup

```csharp
// In your Program.cs or Startup.cs
builder.Services.AddMudServices();

// In your component
<MudThemeManager />
```

## 📋 **Requirements**

- .NET 8.0 or .NET 9.0
- MudBlazor 7.0.0 or later
- Blazor (Server or WebAssembly)

## 🔗 **Links**

- **Repository**: https://github.com/SynapeResearchSystemsCorp/Synapsers.MudBlazor.ThemeManager.Saver
- **NuGet Package**: https://www.nuget.org/packages/Synapsers.MudBlazor.ThemeManager.Saver/
- **Original MudBlazor ThemeManager**: https://github.com/MudBlazor/ThemeManager

## 🙏 **Acknowledgments**

This project is built upon the excellent work of the [MudBlazor Team](https://github.com/MudBlazor/MudBlazor) and their original ThemeManager component. We extend our gratitude for their foundational work that made this enhanced version possible.

## 📄 **License**

This project is licensed under the MIT License - see the LICENSE file for details.

---

**Made with ❤️ by [Synapser Research Systems](https://github.com/SynapeResearchSystemsCorp)**
