using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nutritions.Utility;

namespace Nutritions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
        public class AminoAcidController : Controller
    {
       
            private readonly NutritionDbContext _context;

            public AminoAcidController(NutritionDbContext context)
            {
                _context = context;
            }
            [HttpGet]
            public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? sources)
            {

                var query = _context.AminoAcids.AsQueryable();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query.Where(v => v.Name != null && v.Name.Contains(name));
                }
                if (!string.IsNullOrWhiteSpace(sources))
                {
                    query.Where(v => v.Sources != null && v.Sources.Contains(sources));
                }
            var AminoAcid = await query
                .Select(v => new
                {
                    v.Id,
                    Name = v.Name ?? "N/A",
                    Benefits = v.Benefits ?? "No benefits listed",
                    Rda = v.Rda ?? "Unknown",
                    Sources = v.Sources ?? "Not specified",
                    Diet = v.Diet ?? "Unknown",
                    Supplement_details = v.Supplement_details ?? "N/A"

                })
                .ToListAsync();
                    
                return Ok(AminoAcid);
            }
        }
}
