using System.ComponentModel.DataAnnotations;

namespace LostAndFound.API.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public DateTime ReportedDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public int ReportedByUserId { get; set; }

        public virtual User? ReportedByUser { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}