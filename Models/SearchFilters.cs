namespace LostAndFound.API.Models
{
    public class SearchFilters
    {
        public string? SearchTerm { get; set; }
        public List<string>? Status { get; set; }
        public List<string>? Categories { get; set; }
    }
}