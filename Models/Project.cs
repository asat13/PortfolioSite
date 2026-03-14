namespace PortfolioSiteWasm.Models
{
    public class Project
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = "images/error.svg";

        public string YoutubeURL { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string[] Techs { get; set; } = Array.Empty<string>();
        public string[] Platforms { get; set; } = Array.Empty<string>();
        public string Type { get; set; } = string.Empty;
        public int Year { get; set; } = DateTime.MinValue.Year;
        public string Contribution { get; set; } = string.Empty;
        public string ProjectCategory { get; set; } = string.Empty;
    }
}
