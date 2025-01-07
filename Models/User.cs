using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFound.API.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public string Status { get; set; } = "active";

        public DateTime LastLogin { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Item> ReportedItems { get; set; } = new List<Item>();
        public virtual ICollection<Demande> Demandes { get; set; } = new List<Demande>();

        [InverseProperty("Sender")]
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();

        [InverseProperty("Receiver")]
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    }
}