# Contributing to MudBlazor ThemeManager Saver

Thank you for your interest in contributing to MudBlazor ThemeManager Saver! We welcome contributions from the community and are grateful for any help you can provide.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [How to Contribute](#how-to-contribute)
- [Development Setup](#development-setup)
- [Coding Standards](#coding-standards)
- [Testing](#testing)
- [Submitting Changes](#submitting-changes)
- [Issue Guidelines](#issue-guidelines)
- [Feature Requests](#feature-requests)
- [Documentation](#documentation)

## Code of Conduct

This project adheres to a code of conduct that we expect all contributors to follow. Please be respectful and constructive in all interactions.

### Our Standards

- Use welcoming and inclusive language
- Be respectful of differing viewpoints and experiences
- Gracefully accept constructive criticism
- Focus on what is best for the community
- Show empathy towards other community members

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- Basic knowledge of Blazor and MudBlazor components

### Fork and Clone

1. Fork the repository on GitHub
2. Clone your fork locally:
   ```bash
   git clone https://github.com/YOUR-USERNAME/MudBlazor.ThemeManager.Saver.git
   cd MudBlazor.ThemeManager.Saver
   ```

3. Add the original repository as upstream:
   ```bash
   git remote add upstream https://github.com/ORIGINAL-OWNER/MudBlazor.ThemeManager.Saver.git
   ```

## How to Contribute

### Types of Contributions

We welcome several types of contributions:

- **Bug fixes**: Help us fix issues and improve stability
- **Feature enhancements**: Add new functionality to theme saving capabilities
- **Documentation**: Improve our guides, examples, and API documentation
- **Examples**: Create sample implementations showing how to use the component
- **Performance improvements**: Optimize existing code
- **Testing**: Add or improve unit tests and integration tests

### Areas Where We Need Help

- **Persistence Providers**: Additional storage options (database, cloud storage, etc.)
- **Theme Import/Export**: Support for different theme formats
- **Theme Validation**: Enhanced validation and error handling
- **Performance**: Optimizations for large theme files
- **Accessibility**: Ensuring components meet accessibility standards
- **Mobile Support**: Improving mobile experience for theme management

## Development Setup

1. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

2. **Build the project**:
   ```bash
   dotnet build
   ```

3. **Run tests** (if available):
   ```bash
   dotnet test
   ```

### Project Structure

```
MudBlazor.ThemeManager.Saver/
├── Components/              # Blazor components
│   ├── MudThemeManager.razor
│   ├── MudThemeManagerButton.razor
│   └── MudThemeManagerColorItem.razor
├── Extensions/              # Extension methods and utilities
├── Models/                  # Data models and DTOs
├── wwwroot/                # Static assets (CSS, JS)
├── README.md
└── CONTRIBUTING.md
```

## Coding Standards

### C# Coding Guidelines

- Follow [Microsoft's C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Use `async`/`await` for asynchronous operations
- Prefer `var` when the type is obvious

### Blazor Component Guidelines

- Use meaningful component names with `Mud` prefix
- Follow MudBlazor naming conventions
- Add proper parameter validation
- Include XML documentation for parameters
- Use proper lifecycle methods (`OnInitializedAsync`, `OnAfterRenderAsync`)

### Example Code Style

```csharp
/// <summary>
/// Saves the theme configuration to persistent storage.
/// </summary>
/// <param name="theme">The theme to save</param>
/// <param name="cancellationToken">Cancellation token</param>
/// <returns>A task representing the save operation</returns>
public async Task SaveThemeAsync(ThemeManagerTheme theme, CancellationToken cancellationToken = default)
{
    if (theme == null)
        throw new ArgumentNullException(nameof(theme));

    var json = JsonSerializer.Serialize(theme, _jsonOptions);
    await File.WriteAllTextAsync(_filePath, json, cancellationToken);
}
```

## Testing

### Writing Tests

- Write unit tests for new functionality
- Include integration tests for complex scenarios
- Test error conditions and edge cases
- Use descriptive test method names

### Test Categories

- **Unit Tests**: Test individual methods and components
- **Integration Tests**: Test component interactions
- **Performance Tests**: Ensure acceptable performance
- **Accessibility Tests**: Verify accessibility compliance

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test --filter "Category=Unit"

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Submitting Changes

### Before Submitting

1. **Update from upstream**:
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. **Ensure all tests pass**:
   ```bash
   dotnet test
   ```

3. **Check for build warnings**:
   ```bash
   dotnet build --verbosity normal
   ```

### Pull Request Process

1. **Create a feature branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes** following the coding standards

3. **Commit your changes**:
   ```bash
   git add .
   git commit -m "feat: add theme export functionality"
   ```

4. **Push to your fork**:
   ```bash
   git push origin feature/your-feature-name
   ```

5. **Create a Pull Request** on GitHub

### Commit Message Guidelines

Use [Conventional Commits](https://www.conventionalcommits.org/) format:

- `feat:` for new features
- `fix:` for bug fixes
- `docs:` for documentation changes
- `style:` for formatting changes
- `refactor:` for code refactoring
- `test:` for adding tests
- `chore:` for maintenance tasks

Examples:
```
feat: add database persistence provider
fix: resolve theme serialization issue with null values
docs: update README with new persistence options
```

## Issue Guidelines

### Before Creating an Issue

1. **Search existing issues** to avoid duplicates
2. **Check the documentation** for solutions
3. **Try the latest version** to see if the issue persists

### Creating Good Issues

- **Use a clear, descriptive title**
- **Provide a detailed description** of the problem
- **Include steps to reproduce** the issue
- **Add relevant code samples** or screenshots
- **Specify your environment** (.NET version, browser, OS)

### Issue Templates

#### Bug Report
```markdown
**Describe the bug**
A clear description of what the bug is.

**To Reproduce**
Steps to reproduce the behavior:
1. Go to '...'
2. Click on '....'
3. See error

**Expected behavior**
What you expected to happen.

**Environment:**
- .NET Version: [e.g., 8.0]
- Browser: [e.g., Chrome 120]
- OS: [e.g., Windows 11]
```

#### Feature Request
```markdown
**Is your feature request related to a problem?**
A clear description of what the problem is.

**Describe the solution you'd like**
A clear description of what you want to happen.

**Additional context**
Any other context or screenshots about the feature request.
```

## Feature Requests

We welcome feature requests! When proposing new features:

- **Explain the use case** and why it's valuable
- **Consider the scope** - does it fit the project's goals?
- **Think about implementation** - how complex would it be?
- **Check for existing alternatives** - can it be achieved differently?

### Priority Features

Current areas of focus for new features:
- Enhanced persistence providers
- Theme validation and error handling
- Performance optimizations
- Better mobile support
- Accessibility improvements

## Documentation

### Types of Documentation

- **API Documentation**: XML comments for public APIs
- **User Guides**: Step-by-step instructions in README
- **Examples**: Sample implementations and use cases
- **Code Comments**: Explain complex logic inline

### Documentation Standards

- **Clear and concise**: Use simple, direct language
- **Include examples**: Show how to use features
- **Keep updated**: Update docs when changing functionality
- **Test examples**: Ensure code samples work

### Contributing to Documentation

Documentation improvements are always welcome:
- Fix typos and grammar
- Add missing examples
- Improve clarity of explanations
- Update outdated information

## Getting Help

If you need help with contributing:

- **Check the README**: Start with the project documentation
- **Search issues**: Look for similar questions or problems
- **Create an issue**: Ask questions using the question template
- **Join discussions**: Participate in GitHub Discussions (if available)

## Recognition

We appreciate all contributions and will recognize contributors in:
- Release notes
- Contributors section in README
- GitHub contributor graphs

Thank you for contributing to MudBlazor ThemeManager Saver! 🎨✨
