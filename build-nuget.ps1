# PowerShell script to build NuGet package and update version.json

# Prompt for version
$version = Read-Host "Enter the version you want to build (e.g., 1.0.3)"

# Update version.json with the selected version
$versionJsonPath = "version.json"
$versionJson = Get-Content $versionJsonPath | ConvertFrom-Json
$versionJson.version = $version
$versionJson | ConvertTo-Json -Depth 10 | Set-Content $versionJsonPath -Encoding UTF8

# Update version in badges and documentation
(Get-Content README.md -Raw) -replace '(NuGet version\">)([0-9]+\.[0-9]+\.[0-9]+)', "`$1$version" | Set-Content README.md -Encoding UTF8
(Get-Content src/Synapsers.MudBlazor.ThemeManager.Saver/READMEPACK.md -Raw) -replace '(version": ")([0-9]+\.[0-9]+\.[0-9]+)', "`$1$version" | Set-Content src/Synapsers.MudBlazor.ThemeManager.Saver/READMEPACK.md -Encoding UTF8
(Get-Content src/Synapsers.MudBlazor.ThemeManager.Saver/README.md -Raw) -replace '(version": ")([0-9]+\.[0-9]+\.[0-9]+)', "`$1$version" | Set-Content src/Synapsers.MudBlazor.ThemeManager.Saver/README.md -Encoding UTF8

# Define paths
$projectPath = "src/Synapsers.MudBlazor.ThemeManager.Saver/Synapsers.MudBlazor.ThemeManager.Saver.csproj"
$nupkgOutput = "nupkg"

# Ensure output directory exists
if (!(Test-Path $nupkgOutput)) {
    New-Item -ItemType Directory -Path $nupkgOutput | Out-Null
}

# Clean any existing files for this version
Write-Host "Cleaning existing packages for version $version..."
Remove-Item -Path "$nupkgOutput/Synapsers.MudBlazor.ThemeManager.Saver.$version.*" -Force -ErrorAction SilentlyContinue

# Clean the project first
Write-Host "Cleaning project..."
dotnet clean $projectPath -c Release

# Build the project
Write-Host "Building project with version $version..."
dotnet build $projectPath -c Release

# Pack the project (let Nerdbank.GitVersioning control the version)
Write-Host "Packing NuGet package version $version..."
dotnet pack $projectPath -c Release --output $nupkgOutput

# Verify the package was created
$packagePath = "$nupkgOutput/Synapsers.MudBlazor.ThemeManager.Saver.$version.nupkg"
if (Test-Path $packagePath) {
    Write-Host "Success! Package created at: $packagePath" -ForegroundColor Green
} else {
    Write-Host "Failed to create package. Checking for build output..." -ForegroundColor Red
    
    # Look for the package in alternative locations
    Write-Host "Searching for package in other directories..."
    $foundPackages = Get-ChildItem -Path "." -Recurse -Filter "Synapsers.MudBlazor.ThemeManager.Saver.$version.nupkg"
    
    if ($foundPackages.Count -gt 0) {
        Write-Host "Found package(s) in other locations:" -ForegroundColor Yellow
        foreach ($pkg in $foundPackages) {
            Write-Host $pkg.FullName -ForegroundColor Yellow
            # Copy to the specified output directory
            Copy-Item -Path $pkg.FullName -Destination $nupkgOutput -Force
            Write-Host "Copied to $nupkgOutput folder" -ForegroundColor Green
        }
    } else {
        Write-Host "No packages found for version $version anywhere in the project." -ForegroundColor Red
    }
}

# List all packages in the output directory
Write-Host "Contents of $nupkgOutput folder:"
Get-ChildItem -Path $nupkgOutput | ForEach-Object { Write-Host $_.Name }

# Git commit, tag, and push for the new release
Write-Host "Committing, tagging, and pushing release v$version to git..."
git add .
git commit -m "Release v$version"
git tag v$version
git push
git push --tags
Write-Host "Git release and tag v$version created and pushed."
