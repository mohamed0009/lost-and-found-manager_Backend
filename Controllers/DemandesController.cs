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
    public class DemandesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DemandesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<Demande>>> GetMyDemandes()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            return await _context.Demandes
                .Include(d => d.Item)
                .Include(d => d.RequestedByUser)
                .Where(d => d.RequestedByUserId == userId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Demande>> CreateDemande(Demande demande)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            demande.RequestedByUserId = userId;
            demande.CreatedAt = DateTime.UtcNow;
            demande.UpdatedAt = DateTime.UtcNow;

            _context.Demandes.Add(demande);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMyDemandes), new { id = demande.Id }, demande);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var demande = await _context.Demandes.FindAsync(id);
            if (demande == null)
                return NotFound();

            demande.Status = status;
            demande.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}