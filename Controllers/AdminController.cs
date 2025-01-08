using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LostAndFound.API.Data;
using LostAndFound.API.Models;
using LostAndFound.API.Models.Dtos;
using Microsoft.Extensions.Logging;
using LostAndFound.API.DTOs;
using LostAndFound.API.Services;
using BCrypt.Net;

namespace LostAndFound.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly IImageService _imageService;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger, IImageService imageService)
        {
            _context = context;
            _logger = logger;
            _imageService = imageService;
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

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                {
                    return BadRequest(new { message = "Email already exists" });
                }

                var user = new User
                {
                    Email = userDto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                    Name = userDto.Name,
                    Role = userDto.Role,
                    Status = "active",
                    CreatedAt = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("items")]
        public async Task<IActionResult> CreateItem([FromForm] ItemCreateDto itemDto)
        {
            try
            {
                var item = new Item
                {
                    Description = itemDto.Description,
                    Location = itemDto.Location,
                    Type = itemDto.Type,
                    Category = itemDto.Category,
                    Status = "pending",
                    ReportedDate = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                if (itemDto.Image != null)
                {
                    item.ImageUrl = await _imageService.SaveImageAsync(itemDto.Image);
                }
                else if (!string.IsNullOrEmpty(itemDto.ImageUrl))
                {
                    item.ImageUrl = itemDto.ImageUrl;
                }

                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
    }
}