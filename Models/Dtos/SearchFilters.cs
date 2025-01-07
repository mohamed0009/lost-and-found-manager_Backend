namespace LostAndFound.API.Models.Dtos
{
    public class SearchFilters
    {
        public string? SearchTerm { get; set; }
        public List<string>? Status { get; set; }
        public List<string>? Categories { get; set; }
    }
}