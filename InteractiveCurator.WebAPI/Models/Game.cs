namespace InteractiveCurator.WebAPI.Models
{
    public class Game
    {
        public string AppId { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Genres { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public string ReleaseDate { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public decimal Price { get; set; }
    }
}
