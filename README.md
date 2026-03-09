# Andre Maestas — Portfolio Site

A personal portfolio website built with **Blazor WebAssembly** (.NET 8), showcasing game development and VR projects.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | Blazor WebAssembly (.NET 8) |
| Language | C# |
| Styling | Custom CSS + Bootstrap 5 |
| Fonts | Google Fonts — Syne, DM Sans |
| Data | JSON (static, loaded via HttpClient) |
| Testing | xUnit + bUnit |
| Deployment | FTP → GoDaddy shared hosting |

---

## Project Structure

```
PrtfolioSite/
├── App.razor                    # Root component with routing
├── Program.cs                   # WebAssembly host setup
├── _Imports.razor               # Global using statements
├── PortfolioSiteWasm.csproj     # Main project file
├── PortfolioSiteWasm.slnx       # Solution file
│
├── Layout/
│   ├── MainLayout.razor         # Site-wide layout (header, footer)
│   ├── MainLayout.razor.css
│   ├── NavMenu.razor            # Navigation bar
│   └── NavMenu.razor.css
│
├── Pages/
│   ├── Home.razor               # / — Hero + About sections
│   ├── Projects.razor           # /projects — Filterable project gallery
│   ├── ProjectDetail.razor      # /project/{TitleSlug} — Full project view
│   ├── Contact.razor            # /contact — Contact info
│   ├── Counter.razor            # /counter — Blazor demo
│   └── Error.razor              # Error page
│
├── Models/
│   └── Project.cs               # Project data model
│
├── Reusable/
│   ├── ProjectView.razor        # Project card component
│   └── ProjectView.razor.css
│
├── wwwroot/
│   ├── index.html               # HTML shell
│   ├── app.css                  # Global styles
│   ├── favicon.png
│   ├── resume.pdf               # Downloadable resume
│   ├── bootstrap/
│   ├── data/
│   │   └── projects.json        # Project data (edit here to add/update projects)
│   └── images/
│
├── Tests/                       # Test project
│   ├── PortfolioSiteWasm.Tests.csproj
│   ├── GlobalUsings.cs
│   ├── ProjectModelTests.cs
│   ├── ProjectViewTests.cs
│   ├── ProjectsFilterTests.cs
│   └── ProjectDetailTests.cs
│
└── .vscode/
    ├── launch.json              # Debug + test launch configs
    └── tasks.json               # Build, publish, watch, test tasks
```

---

## Pages & Routes

| Route | Page | Description |
|---|---|---|
| `/` | Home | Hero section with intro + About section with skills |
| `/projects` | Projects | Filterable grid of projects (search, tech, type filters) |
| `/project/{TitleSlug}` | ProjectDetail | Full detail view for a single project |
| `/contact` | Contact | Email, GitHub, LinkedIn links |

---

## Adding or Editing Projects

All project data lives in [`wwwroot/data/projects.json`](wwwroot/data/projects.json). Each entry follows the `Project` model:

```json
{
  "Title": "My Project",
  "Description": "Short description shown on the card.",
  "LongDescription": "Extended description shown on the detail page.",
  "ImageSrc": "images/myproject.png",
  "YoutubeURL": "https://www.youtube.com/embed/VIDEO_ID",
  "URL": "https://external-link.com",
  "Techs": ["C#", "Unity"],
  "Platforms": ["PC", "VR"],
  "Type": "Game",
  "Year": 2024
}
```

Drop the project image in `wwwroot/images/` and reference it in `ImageSrc`.

---

## Running Locally

**Prerequisites:** .NET 8 SDK

```bash
# Run with hot reload
dotnet watch run

# Or just run
dotnet run
```

The app will be available at `https://localhost:5001` (or the port shown in the terminal).

In VS Code, press **F5** and select **Launch and Debug Blazor WebAssembly Application** to run with the debugger attached in Chrome.

---

## Running Tests

```bash
dotnet test Tests/PortfolioSiteWasm.Tests.csproj
```

Or use the VS Code task: `Ctrl+Shift+P` → **Tasks: Run Test Task**

For watch mode (reruns on save):

```bash
dotnet watch test --project Tests/PortfolioSiteWasm.Tests.csproj
```

### Test Coverage

| File | What's tested |
|---|---|
| `ProjectModelTests` | Model defaults and property assignment |
| `ProjectViewTests` | Card rendering, tech badges, URL encoding, null safety |
| `ProjectsFilterTests` | Filter by name/tech/type, multi-filter AND logic, clear filters, toggle panel, loading state |
| `ProjectDetailTests` | Found/not found states, description fallback, chips, external link, back navigation |

---

## Deployment

Build a release-optimised bundle:

```bash
dotnet publish -c Release
```

Upload the contents of `bin/Release/net8.0/publish/wwwroot/` to `public_html/` on the hosting server via FTP.
