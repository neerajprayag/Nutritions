using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nutritions.Utility;

namespace Nutritions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MineralController : Controller
    {
        private readonly NutritionDbContext _context;

        public MineralController(NutritionDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? sources)
        {
            
            var query = _context.Minerals.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name)) {
                query.Where(v => v.Name != null && v.Name.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(sources))
            {
                query.Where(v => v.Sources != null && v.Sources.Contains(sources));
            }
            var mineral = await query
                .Select(v => new 
                { v.Id,
                  Name = v.Name ?? "N/A",
                  Benefits = v.Benefits ?? "No benefits listed",
                  Rda = v.Rda ?? "Unknown",
                  Sources = v.Sources ?? "Not specified",
                  Diet = v.Diet ?? "Unknown",
                    Supplement_details = v.Supplement_details ?? "N/A"

                })
                .ToListAsync();
            return Ok(mineral);
        }
            
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
