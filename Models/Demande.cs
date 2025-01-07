using System;
using System.Collections.Generic;
using LostAndFound.API.Models;

namespace LostAndFound.API.Models
{
    public class Demande
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int RequestedByUserId { get; set; }
        public string Status { get; set; } = "pending"; // pending, approved, rejected
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual Item? Item { get; set; }
        public virtual User? RequestedByUser { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
    }
}