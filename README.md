# Run and debug (VS Code)

Quick steps to build and run the project locally.

1. Install the recommended extension: C# (ms-dotnettools.csharp).
2. Build the project:

```powershell
dotnet build "G:\OneDrive\PortfolioWebsiteProject\PortfolioWebsiteProject.csproj"
```

3. Run the project (foreground):

```powershell
dotnet run --project "G:\OneDrive\PortfolioWebsiteProject\PortfolioWebsiteProject.csproj"
```

4. In VS Code, open the Run and Debug view and select ".NET Core Launch (web)", then Start Debugging (F5).

Notes:

- The `launch.json` expects the build output at `bin/Debug/net8.0/PortfolioWebsiteProject.dll`.
- If you target a different framework or configuration, update `.vscode/launch.json` accordingly.
