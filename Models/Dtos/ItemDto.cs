namespace LostAndFound.API.Models.Dtos
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ReportedDate { get; set; }
        public int ReportedByUserId { get; set; }
        public string ReportedByUserName { get; set; }
    }
}