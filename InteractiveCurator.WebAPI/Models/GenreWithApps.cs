namespace InteractiveCurator.WebAPI.Models
{
    public class GenreWithApps
    {
        public string Genre { get; set; }
        public List<Dictionary<string, object>> Apps { get; set; }
    }
}
