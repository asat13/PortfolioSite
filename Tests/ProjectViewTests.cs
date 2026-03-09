using Bunit;
using PortfolioSiteWasm.Models;
using PortfolioSiteWasm.Reusable;

namespace PortfolioSiteWasm.Tests;

public class ProjectViewTests : TestContext
{
    private static Project Make(
        string title = "Test Project",
        string description = "A description",
        string[]? techs = null,
        string type = "Game",
        int year = 2024) => new Project
        {
            Title = title,
            Description = description,
            ImageSrc = "images/test.png",
            Techs = techs ?? [],
            Type = type,
            Year = year
        };

    [Fact]
    public void RendersTitle()
    {
        var cut = RenderComponent<ProjectView>(p => p.Add(x => x.Project, Make("My Game")));
        Assert.Equal("My Game", cut.Find("h3.pv-title").TextContent);
    }

    [Fact]
    public void RendersDescription()
    {
        var cut = RenderComponent<ProjectView>(p => p.Add(x => x.Project, Make(description: "Cool game")));
        Assert.Contains("Cool game", cut.Find(".pv-description").TextContent);
    }

    [Fact]
    public void RendersTechBadges_WhenTechsPresent()
    {
        var cut = RenderComponent<ProjectView>(p => p.Add(x => x.Project, Make(techs: ["C#", "Unity"])));
        var badges = cut.FindAll(".pv-tech-badge");
        Assert.Equal(2, badges.Count);
        Assert.Equal("C#", badges[0].TextContent);
        Assert.Equal("Unity", badges[1].TextContent);
    }

    [Fact]
    public void HidesTechSection_WhenNoTechs()
    {
        var cut = RenderComponent<ProjectView>(p => p.Add(x => x.Project, Make(techs: [])));
        Assert.Empty(cut.FindAll(".pv-tech-badge"));
    }

    [Fact]
    public void RendersYearAndType_WhenTechsPresent()
    {
        var cut = RenderComponent<ProjectView>(p => p.Add(x => x.Project, Make(techs: ["Unity"], type: "Game", year: 2023)));
        var meta = cut.FindAll(".pv-meta .pv-description");
        Assert.Contains(meta, m => m.TextContent.Contains("2023"));
        Assert.Contains(meta, m => m.TextContent.Contains("Game"));
    }

    [Fact]
    public void ViewProjectLink_PointsToCorrectRoute()
    {
        var cut = RenderComponent<ProjectView>(p => p.Add(x => x.Project, Make("My Project")));
        var href = cut.Find("a.pv-button").GetAttribute("href");
        Assert.Equal($"/project/{Uri.EscapeDataString("My Project")}", href);
    }

    [Fact]
    public void ViewProjectLink_EncodesSpecialCharacters()
    {
        var cut = RenderComponent<ProjectView>(p => p.Add(x => x.Project, Make("C# & Blazor")));
        var href = cut.Find("a.pv-button").GetAttribute("href");
        Assert.Equal($"/project/{Uri.EscapeDataString("C# & Blazor")}", href);
    }

    [Fact]
    public void NullProject_DoesNotThrow()
    {
        var ex = Record.Exception(() => RenderComponent<ProjectView>(p => p.Add(x => x.Project, (Project?)null)));
        Assert.Null(ex);
    }
}
