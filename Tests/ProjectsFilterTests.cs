using System.Net;
using System.Text;
using System.Text.Json;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using PortfolioSiteWasm.Models;
using PortfolioSiteWasm.Pages;

namespace PortfolioSiteWasm.Tests;

public class ProjectsFilterTests : TestContext
{
    private static readonly Project[] SampleProjects =
    [
        new() { Title = "Alpha", Type = "Game", Techs = ["C#", "Unity"], Year = 2023 },
        new() { Title = "Beta",  Type = "Web",  Techs = ["Blazor", "C#"], Year = 2024 },
        new() { Title = "Gamma", Type = "Game", Techs = ["Unity"], Year = 2022 },
    ];

    private void RegisterProjects(Project[] projects)
    {
        var json = JsonSerializer.Serialize(projects);
        Services.AddSingleton(new HttpClient(new FakeHttpHandler(json))
        {
            BaseAddress = new Uri("http://localhost/")
        });
    }

    [Fact]
    public void ShowsAllProjects_WhenNoFiltersApplied()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<Projects>();
        cut.WaitForAssertion(() => Assert.Equal(3, cut.FindAll(".project-card").Count));
    }

    [Fact]
    public void ShowsLoadingState_BeforeHttpCompletes()
    {
        var tcs = new TaskCompletionSource<HttpResponseMessage>();
        Services.AddSingleton(new HttpClient(new TcsHttpHandler(tcs))
        {
            BaseAddress = new Uri("http://localhost/")
        });

        var cut = RenderComponent<Projects>();
        Assert.NotEmpty(cut.FindAll(".skeleton-card"));
    }

    [Fact]
    public void ShowsEmptyMessage_WhenNoProjectsMatchFilter()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<Projects>();
        cut.WaitForAssertion(() => Assert.Equal(3, cut.FindAll(".project-card").Count));

        cut.Find("input.filter-search").Input("ZZZNoMatch");
        Assert.Contains("No projects match", cut.Find(".projects-empty").TextContent);
    }

    [Fact]
    public void FiltersByName_CaseInsensitive()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<Projects>();
        cut.WaitForAssertion(() => Assert.Equal(3, cut.FindAll(".project-card").Count));

        cut.Find("input.filter-search").Input("alpha");
        Assert.Single(cut.FindAll(".project-card"));
    }

    [Fact]
    public void FiltersByTech_ShowsOnlyMatchingProjects()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<Projects>();
        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll(".pill")));

        cut.FindAll(".pill").First(p => p.TextContent.Trim() == "Blazor").Click();
        Assert.Single(cut.FindAll(".project-card"));
    }

    [Fact]
    public void FiltersByType_ShowsOnlyMatchingProjects()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<Projects>();
        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll(".pill")));

        cut.FindAll(".pill").First(p => p.TextContent.Trim() == "Web").Click();
        Assert.Single(cut.FindAll(".project-card"));
    }

    [Fact]
    public void MultipleTechFilters_RequireAllToMatch()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<Projects>();
        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll(".pill")));

        // Select both "C#" and "Blazor" — only Beta has both
        cut.FindAll(".pill").First(p => p.TextContent.Trim() == "C#").Click();
        cut.FindAll(".pill").First(p => p.TextContent.Trim() == "Blazor").Click();
        Assert.Single(cut.FindAll(".project-card"));
    }

    [Fact]
    public void ActiveFilterCount_IncrementsWithNameFilter()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<Projects>();
        cut.WaitForAssertion(() => Assert.Equal(3, cut.FindAll(".project-card").Count));

        Assert.Empty(cut.FindAll(".filters-badge"));
        cut.Find("input.filter-search").Input("alpha");
        Assert.Equal("1", cut.Find(".filters-badge").TextContent);
    }

    [Fact]
    public void ActiveFilterCount_IncrementsWithTechAndTypeFilters()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<Projects>();
        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll(".pill")));

        cut.FindAll(".pill").First(p => p.TextContent.Trim() == "C#").Click();
        cut.FindAll(".pill").First(p => p.TextContent.Trim() == "Game").Click();
        Assert.Equal("2", cut.Find(".filters-badge").TextContent);
    }

    [Fact]
    public void ClearFilters_ResetsAllFiltersAndShowsAllProjects()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<Projects>();
        cut.WaitForAssertion(() => Assert.Equal(3, cut.FindAll(".project-card").Count));

        cut.Find("input.filter-search").Input("alpha");
        Assert.Single(cut.FindAll(".project-card"));

        cut.Find("button.btn-clear").Click();
        Assert.Equal(3, cut.FindAll(".project-card").Count);
        Assert.Empty(cut.FindAll(".filters-badge"));
    }

    [Fact]
    public void ToggleFilters_HidesAndShowsFilterPanel()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<Projects>();

        Assert.NotEmpty(cut.FindAll("#filters-body"));
        cut.Find("button.filters-toggle").Click();
        Assert.Empty(cut.FindAll("#filters-body"));
        cut.Find("button.filters-toggle").Click();
        Assert.NotEmpty(cut.FindAll("#filters-body"));
    }

    private sealed class FakeHttpHandler : HttpMessageHandler
    {
        private readonly string _json;
        public FakeHttpHandler(string json) => _json = json;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_json, Encoding.UTF8, "application/json")
            });
    }

    private sealed class TcsHttpHandler : HttpMessageHandler
    {
        private readonly TaskCompletionSource<HttpResponseMessage> _tcs;
        public TcsHttpHandler(TaskCompletionSource<HttpResponseMessage> tcs) => _tcs = tcs;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct) =>
            _tcs.Task;
    }
}
