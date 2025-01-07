using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LostAndFound.API.Data;
using LostAndFound.API.Models;
using LostAndFound.API.Models.Dtos;
using Microsoft.Extensions.Logging;

namespace LostAndFound.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = u.Name,
                    Role = u.Role,
                    Status = u.Status
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPut("users/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] string status)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Status = status;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Role = userDto.Role;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems()
        {
            try
            {
                var items = await _context.Items
                    .Include(i => i.ReportedByUser)
                    .OrderByDescending(i => i.ReportedDate)
                    .Select(i => new ItemDto
                    {
                        Id = i.Id,
                        Description = i.Description,
                        Location = i.Location,
                        Type = i.Type,
                        Status = i.Status,
                        Category = i.Category,
                        ImageUrl = i.ImageUrl,
                        ReportedDate = i.ReportedDate,
                        ReportedByUserId = i.ReportedByUserId,
                        ReportedByUserName = i.ReportedByUser != null ? i.ReportedByUser.Name : "Inconnu"
                    })
                    .ToListAsync();

                if (!items.Any())
                {
                    return Ok(new List<ItemDto>()); // Retourne une liste vide si aucun item
                }

                return Ok(items);
            }
            catch (Exception ex)
            {
                // Log l'erreur
                _logger.LogError(ex, "Erreur lors de la récupération des items");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des items");
            }
        }

        [HttpPut("items/{id}/status")]
        public async Task<IActionResult> UpdateItemStatus(int id, [FromBody] string status)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound();

            item.Status = status;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}