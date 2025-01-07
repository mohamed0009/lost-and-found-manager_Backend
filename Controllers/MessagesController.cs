using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LostAndFound.API.Models;
using LostAndFound.API.Data;
using System.Security.Claims;

namespace LostAndFound.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("demande/{demandeId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByDemande(int demandeId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var demande = await _context.Demandes
                .FirstOrDefaultAsync(d => d.Id == demandeId &&
                    (d.RequestedByUserId == userId || User.IsInRole("Admin")));

            if (demande == null)
                return NotFound();

            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.DemandeId == demandeId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Message>> SendMessage(Message message)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            message.SenderId = userId;
            message.CreatedAt = DateTime.UtcNow;
            message.IsRead = false;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMessagesByDemande),
                new { demandeId = message.DemandeId }, message);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return NotFound();

            if (message.ReceiverId != userId && !User.IsInRole("Admin"))
                return Forbid();

            message.IsRead = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}