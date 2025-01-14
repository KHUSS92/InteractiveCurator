namespace InteractiveCurator.WebAPI.Models
{
    public class AppDetail
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public List<string> Genres { get; set; }
    }
}
