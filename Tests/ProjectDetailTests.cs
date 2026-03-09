using System.Net;
using System.Text;
using System.Text.Json;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using PortfolioSiteWasm.Models;
using PortfolioSiteWasm.Pages;

namespace PortfolioSiteWasm.Tests;

public class ProjectDetailTests : TestContext
{
    private static readonly Project[] SampleProjects =
    [
        new()
        {
            Title = "Survival VR",
            Description = "Short desc",
            LongDescription = "A longer description about Survival VR",
            ImageSrc = "images/survivalvr.png",
            Techs = ["C#", "Unity"],
            Platforms = ["PC", "VR"],
            Type = "Game",
            Year = 2023,
            URL = "https://example.com"
        },
        new()
        {
            Title = "Portfolio Site",
            Description = "This website",
            Techs = ["Blazor", "C#"],
            Type = "Web",
            Year = 2024
        }
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
    public void ShowsLoadingState_BeforeHttpCompletes()
    {
        var tcs = new TaskCompletionSource<HttpResponseMessage>();
        Services.AddSingleton(new HttpClient(new TcsHttpHandler(tcs))
        {
            BaseAddress = new Uri("http://localhost/")
        });

        var cut = RenderComponent<ProjectDetail>(p => p.Add(x => x.TitleSlug, "Survival%20VR"));
        Assert.Contains("Loading", cut.Find(".pd-state-msg").TextContent);
    }

    [Fact]
    public void ShowsProjectDetails_WhenFound()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<ProjectDetail>(p =>
            p.Add(x => x.TitleSlug, Uri.EscapeDataString("Survival VR")));

        cut.WaitForAssertion(() => Assert.NotNull(cut.Find(".pd-title")));
        Assert.Equal("Survival VR", cut.Find(".pd-title").TextContent);
    }

    [Fact]
    public void ShowsLongDescription_WhenAvailable()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<ProjectDetail>(p =>
            p.Add(x => x.TitleSlug, Uri.EscapeDataString("Survival VR")));

        cut.WaitForAssertion(() => Assert.NotNull(cut.Find(".pd-body")));
        Assert.Contains("A longer description about Survival VR", cut.Find(".pd-body").TextContent);
    }

    [Fact]
    public void FallsBackToShortDescription_WhenNoLongDescription()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<ProjectDetail>(p =>
            p.Add(x => x.TitleSlug, Uri.EscapeDataString("Portfolio Site")));

        cut.WaitForAssertion(() => Assert.NotNull(cut.Find(".pd-body")));
        Assert.Contains("This website", cut.Find(".pd-body").TextContent);
    }

    [Fact]
    public void ShowsNotFound_WhenTitleDoesNotMatch()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<ProjectDetail>(p =>
            p.Add(x => x.TitleSlug, "NonExistentProject"));

        cut.WaitForAssertion(() => Assert.Contains("not found", cut.Find(".pd-state-msg").TextContent));
    }

    [Fact]
    public void RendersTechChips()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<ProjectDetail>(p =>
            p.Add(x => x.TitleSlug, Uri.EscapeDataString("Survival VR")));

        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll(".pd-chip")));
        var chips = cut.FindAll(".pd-chip");
        Assert.Contains(chips, c => c.TextContent == "C#");
        Assert.Contains(chips, c => c.TextContent == "Unity");
    }

    [Fact]
    public void RendersPlatformChips()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<ProjectDetail>(p =>
            p.Add(x => x.TitleSlug, Uri.EscapeDataString("Survival VR")));

        cut.WaitForAssertion(() => Assert.NotEmpty(cut.FindAll(".pd-chip--platform")));
        var platforms = cut.FindAll(".pd-chip--platform");
        Assert.Contains(platforms, c => c.TextContent == "PC");
        Assert.Contains(platforms, c => c.TextContent == "VR");
    }

    [Fact]
    public void ShowsExternalLink_WhenUrlPresent()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<ProjectDetail>(p =>
            p.Add(x => x.TitleSlug, Uri.EscapeDataString("Survival VR")));

        cut.WaitForAssertion(() => Assert.NotNull(cut.Find(".pd-external-btn")));
        Assert.Equal("https://example.com", cut.Find(".pd-external-btn").GetAttribute("href"));
    }

    [Fact]
    public void HidesExternalLink_WhenNoUrl()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<ProjectDetail>(p =>
            p.Add(x => x.TitleSlug, Uri.EscapeDataString("Portfolio Site")));

        cut.WaitForAssertion(() => Assert.NotNull(cut.Find(".pd-title")));
        Assert.Empty(cut.FindAll(".pd-external-btn"));
    }

    [Fact]
    public void BackLink_AlwaysPointsToProjectsPage()
    {
        RegisterProjects(SampleProjects);
        var cut = RenderComponent<ProjectDetail>(p =>
            p.Add(x => x.TitleSlug, Uri.EscapeDataString("Survival VR")));

        Assert.Equal("/projects", cut.Find("a.pd-back").GetAttribute("href"));
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
