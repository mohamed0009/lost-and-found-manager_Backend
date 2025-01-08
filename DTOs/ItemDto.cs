namespace LostAndFound.API.DTOs
{
    public class ItemCreateDto
    {
        public string Description { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class ItemUpdateDto
    {
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Type { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}