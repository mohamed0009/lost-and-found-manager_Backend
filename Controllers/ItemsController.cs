using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LostAndFound.API.Models;
using LostAndFound.API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.IO;

namespace LostAndFound.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ItemsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            try
            {
                var items = await _context.Items
                    .Include(i => i.ReportedByUser)
                    .Select(i => new
                    {
                        i.Id,
                        i.Description,
                        i.Location,
                        i.ReportedDate,
                        i.Status,
                        i.Type,
                        i.Category,
                        i.ImageUrl,
                        ReportedByUser = new
                        {
                            i.ReportedByUser.Id,
                            i.ReportedByUser.Name,
                            i.ReportedByUser.Email
                        }
                    })
                    .ToListAsync();

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erreur lors du chargement des objets",
                    error = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            try
            {
                var item = await _context.Items
                    .Include(i => i.ReportedByUser)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (item == null)
                {
                    return NotFound(new { message = "Objet non trouvé" });
                }

                return Ok(new
                {
                    id = item.Id,
                    description = item.Description,
                    location = item.Location,
                    reportedDate = item.ReportedDate,
                    status = item.Status,
                    type = item.Type,
                    category = item.Category,
                    imageUrl = item.ImageUrl,
                    updatedAt = item.ReportedDate,
                    reportedByUser = new
                    {
                        id = item.ReportedByUser.Id,
                        name = item.ReportedByUser.Name,
                        email = item.ReportedByUser.Email
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erreur lors du chargement de l'objet",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Item>> CreateItem([FromBody] Item item)
        {
            // Set default values
            item.ReportedDate = DateTime.UtcNow;
            item.Status = "pending";

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            // Return the created item with its new ID
            return CreatedAtAction(
                nameof(GetItem),
                new { id = item.Id },
                item
            );
        }

        [HttpPost("upload")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "No file uploaded" });

            try
            {
                // Validate file size
                if (file.Length > 5 * 1024 * 1024)
                    return BadRequest(new { success = false, message = "File size exceeds 5MB limit" });

                // Validate file type
                var allowedTypes = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedTypes.Contains(extension))
                    return BadRequest(new { success = false, message = "Only .jpg, .jpeg and .png files are allowed" });

                // Ensure directory exists
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                // Create unique filename
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsDir, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return URL
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var imageUrl = $"{baseUrl}/uploads/{fileName}";

                return Ok(new { success = true, imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            if (id != item.Id)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Item>>> SearchItems([FromQuery] string q)
        {
            return await _context.Items
                .Include(i => i.ReportedByUser)
                .Where(i => i.Description.Contains(q) || i.Location.Contains(q))
                .ToListAsync();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }

        // Add other CRUD operations
    }
}