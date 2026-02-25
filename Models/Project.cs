namespace PortfolioWebsiteProject.Models
{
    public class Project
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = "images/error.svg";
        public string URL { get; set; } = string.Empty;
        public string[] Techs { get; set; } = System.Array.Empty<string>();
        public string Type { get; set; } = string.Empty;
        public int Year { get; set; } = DateTime.MinValue.Year;
    }
}
