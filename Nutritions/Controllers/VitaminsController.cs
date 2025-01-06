using Microsoft.AspNetCore.Mvc;
using Nutritions.Utility;
using Microsoft.EntityFrameworkCore;

namespace Nutritions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VitaminsController : ControllerBase
    {
        private readonly NutritionDbContext _context;

        public VitaminsController(NutritionDbContext context)
        {
            _context = context;
        }

        // GET: api/vitamins?name=VitaminC&source=Citrus
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? source)
        {
            // Start with the base query
            var query = _context.Vitamins.AsQueryable();

            // Apply Name filter if provided
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(v => v.Name != null && v.Name.Contains(name));
            }

            // Apply Sources filter if provided
            if (!string.IsNullOrWhiteSpace(source))
            {
                query = query.Where(v => v.Sources != null && v.Sources.Contains(source));
            }

            // Select and transform the data
            var vitamins = await query
                .Select(v => new
                {
                    v.Id,
                    Name = v.Name ?? "N/A",
                    Benefits = v.Benefits ?? "No benefits listed",
                    Rda = v.Rda ?? "Unknown",
                    Sources = v.Sources ?? "Not specified",
                    Diet = v.Diet ?? "Unknown",
                    Supplement_Details = v.Supplement_Details ?? "No details available"
                })
                .OrderByDescending(v => v.Name)
                .ToListAsync();

            return Ok(vitamins);
        }
    }
}
