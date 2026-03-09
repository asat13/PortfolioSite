using PortfolioSiteWasm.Models;

namespace PortfolioSiteWasm.Tests;

public class ProjectModelTests
{
    [Fact]
    public void DefaultImageSrc_IsErrorPlaceholder()
    {
        var project = new Project();
        Assert.Equal("images/error.svg", project.ImageSrc);
    }

    [Fact]
    public void DefaultYear_IsMinYear()
    {
        var project = new Project();
        Assert.Equal(DateTime.MinValue.Year, project.Year);
    }

    [Fact]
    public void DefaultTechs_IsEmptyArray()
    {
        var project = new Project();
        Assert.Empty(project.Techs);
    }

    [Fact]
    public void DefaultPlatforms_IsEmptyArray()
    {
        var project = new Project();
        Assert.Empty(project.Platforms);
    }

    [Fact]
    public void DefaultTitle_IsEmptyString()
    {
        var project = new Project();
        Assert.Equal(string.Empty, project.Title);
    }

    [Fact]
    public void DefaultDescription_IsEmptyString()
    {
        var project = new Project();
        Assert.Equal(string.Empty, project.Description);
    }

    [Fact]
    public void PropertiesCanBeSet()
    {
        var project = new Project
        {
            Title = "Survival VR",
            Description = "A VR survival game",
            LongDescription = "An extended description",
            ImageSrc = "images/survivalvr.png",
            YoutubeURL = "https://youtube.com/embed/abc",
            URL = "https://example.com",
            Techs = ["C#", "Unity"],
            Platforms = ["PC", "VR"],
            Type = "Game",
            Year = 2024
        };

        Assert.Equal("Survival VR", project.Title);
        Assert.Equal("A VR survival game", project.Description);
        Assert.Equal("An extended description", project.LongDescription);
        Assert.Equal("images/survivalvr.png", project.ImageSrc);
        Assert.Equal("https://youtube.com/embed/abc", project.YoutubeURL);
        Assert.Equal("https://example.com", project.URL);
        Assert.Equal(new[] { "C#", "Unity" }, project.Techs);
        Assert.Equal(new[] { "PC", "VR" }, project.Platforms);
        Assert.Equal("Game", project.Type);
        Assert.Equal(2024, project.Year);
    }
}
