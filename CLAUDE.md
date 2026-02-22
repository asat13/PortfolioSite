# CLAUDE.md - Portfolio Website Project

## Project Overview

Personal portfolio website for Andre Maestas built with **ASP.NET Core 8.0 Blazor**. Showcases projects with interactive filtering by name, technology, and type.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 8.0 |
| UI | Blazor Server-Side (Razor Components) |
| Language | C# |
| CSS | Bootstrap 5 + custom `app.css` |
| Data | JSON (`Data/projects.json`) |
| Build | `dotnet` CLI |

## Project Structure

```
PortfolioWebsiteProject/
├── Components/
│   ├── Pages/
│   │   ├── Home.razor         # Landing page (/, hero + social links)
│   │   ├── Projects.razor     # /projects — filtered project listing
│   │   ├── Counter.razor      # /counter — interactive demo
│   │   └── Error.razor        # Error handler
│   ├── Layout/
│   │   ├── MainLayout.razor   # Master layout (nav + content area)
│   │   └── NavMenu.razor      # Responsive hamburger nav
│   ├── Reusable/
│   │   └── ProjectView.razor  # Project card component
│   ├── App.razor              # Root HTML document
│   ├── Routes.razor           # Router config
│   └── _Imports.razor         # Global usings
├── Models/
│   └── Project.cs             # Project data model
├── Data/
│   └── projects.json          # Project data (loaded at runtime)
├── wwwroot/
│   ├── app.css                # Custom styles
│   ├── bootstrap/             # Bootstrap 5 CSS
│   └── images/                # Static images (Home/, Nav/)
├── Program.cs                 # App entry point / DI setup
├── PortfolioWebsiteProject.csproj
└── PortfolioWebsiteProject.sln
```

## Build & Run

```bash
# Build
dotnet build "PortfolioWebsiteProject.csproj"

# Run
dotnet run --project "PortfolioWebsiteProject.csproj"
```

- HTTP: http://localhost:5112
- HTTPS: https://localhost:7124

**VS Code:** Open Run & Debug → select ".NET Core Launch (web)" → F5

## Key Conventions

- **Pages** live in `Components/Pages/` with `@page "/route"` directives
- **Reusable components** go in `Components/Reusable/`
- **Data model** is `Models/Project.cs` — all project fields are non-nullable with defaults
- **Project data** is stored in `Data/projects.json` and deserialized at runtime in `Projects.razor`
- Filtering in `Projects.razor` uses AND logic across selected technologies and types, case-insensitive name search
- No external NuGet packages beyond the .NET Web SDK

## Data Model

```csharp
public class Project
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageSrc { get; set; } = "images/error.svg";
    public string ButtonText { get; set; } = string.Empty;
    public string ButtonUrl { get; set; } = string.Empty;
    public string[] Techs { get; set; } = Array.Empty<string>();
    public string Type { get; set; } = string.Empty;
}
```

## Routes

| Route | Component | Description |
|-------|-----------|-------------|
| `/` | `Home.razor` | Hero / about section |
| `/projects` | `Projects.razor` | Project listing with filters |
| `/counter` | `Counter.razor` | Interactive demo |
| `/contact` | *(not yet implemented)* | Linked in nav |
