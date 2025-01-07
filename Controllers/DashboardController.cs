using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.API.Data;
using LostAndFound.API.Models;
using LostAndFound.API.Models.Dtos;

namespace LostAndFound.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStats>> GetStats()
        {
            var stats = new DashboardStats
            {
                TotalItems = await _context.Items.CountAsync(),
                LostItems = await _context.Items.CountAsync(i => i.Type == "Lost"),
                FoundItems = await _context.Items.CountAsync(i => i.Type == "Found"),
                PendingItems = await _context.Items.CountAsync(i => i.Status == "pending")
            };

            return stats;
        }
    }
}