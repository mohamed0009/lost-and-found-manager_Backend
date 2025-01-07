using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LostAndFound.API.Models;

namespace LostAndFound.API.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int DemandeId { get; set; }

        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }

        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        public virtual Demande? Demande { get; set; }

        [InverseProperty("SentMessages")]
        public virtual User? Sender { get; set; }

        [InverseProperty("ReceivedMessages")]
        public virtual User? Receiver { get; set; }
    }
}