using Microsoft.AspNetCore.Mvc;
//using Nutritions.Model; // Update namespace according to your project
//using Nutritions.Data;   // Update namespace for DbContext
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nutritions.Model;
using Nutritions.Utility;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;


[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly NutritionDbContext _context;

    public AdminController(NutritionDbContext context)
    {
        _context = context;
    }

    // Add Vitamin
    [HttpPost("vitamins")]
    public async Task<IActionResult> AddVitamin([FromBody] Vitamin vitamin)
    {
        _context.Vitamins.Add(vitamin);
        await _context.SaveChangesAsync();
        return Ok(vitamin);
    }

    // Update Vitamin
    [HttpPut("vitamins/{id}")]
    public async Task<IActionResult> UpdateVitamin(int id, [FromBody] Vitamin updatedVitamin)
    {
        var vitamin = await _context.Vitamins.FindAsync(id);
        if (vitamin == null) return NotFound();

        vitamin.Name = updatedVitamin.Name;
        vitamin.Benefits = updatedVitamin.Benefits;
        vitamin.Rda = updatedVitamin.Rda;
        vitamin.Sources = updatedVitamin.Sources;
        vitamin.Supplement_Details = updatedVitamin.Supplement_Details;

        _context.Vitamins.Update(vitamin);
        await _context.SaveChangesAsync();

        return Ok(vitamin);
    }

    // Delete Vitamin
    [HttpDelete("vitamins/{id}")]
    public async Task<IActionResult> DeleteVitamin(int id)
    {
        var vitamin = await _context.Vitamins.FindAsync(id);
        if (vitamin == null) return NotFound();

        _context.Vitamins.Remove(vitamin);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // Similar endpoints for Minerals
    [HttpPost("minerals")]
    public async Task<IActionResult> AddMineral([FromBody] Mineral mineral)
    {
        _context.Minerals.Add(mineral);
        await _context.SaveChangesAsync();
        return Ok(mineral);
    }

    [HttpPut("minerals/{id}")]
    public async Task<IActionResult> UpdateMineral(int id, [FromBody] Mineral updatedMineral)
    {
        var mineral = await _context.Minerals.FindAsync(id);
        if (mineral == null) return NotFound();

        mineral.Name = updatedMineral.Name;
        mineral.Benefits = updatedMineral.Benefits;
        mineral.Rda = updatedMineral.Rda;
        mineral.Sources = updatedMineral.Sources;

        _context.Minerals.Update(mineral);
        await _context.SaveChangesAsync();

        return Ok(mineral);
    }

    [HttpDelete("minerals/{id}")]
    public async Task<IActionResult> DeleteMineral(int id)
    {
        var mineral = await _context.Minerals.FindAsync(id);
        if (mineral == null) return NotFound();

        _context.Minerals.Remove(mineral);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // Similar endpoints for AminoAcid
    [HttpPost("aminoacids")]
    public async Task<IActionResult> AddAminoAcid([FromBody] AminoAcids aminoAcid)
    {
        _context.AminoAcids.Add(aminoAcid);
        await _context.SaveChangesAsync();
        return Ok(aminoAcid);
    }

    [HttpPut("aminoacids/{id}")]
    public async Task<IActionResult> UpdateAminoAcid(int id, [FromBody] AminoAcids updatedAminoAcid)
    {
        var aminoAcid = await _context.AminoAcids.FindAsync(id);
        if (aminoAcid == null) return NotFound();

        aminoAcid.Name = updatedAminoAcid.Name;
        aminoAcid.Benefits = updatedAminoAcid.Benefits;
        aminoAcid.Sources = updatedAminoAcid.Sources;

        _context.AminoAcids.Update(aminoAcid);
        await _context.SaveChangesAsync();

        return Ok(aminoAcid);
    }

    [HttpDelete("aminoacids/{id}")]
    public async Task<IActionResult> DeleteAminoAcid(int id)
    {
        var aminoAcid = await _context.AminoAcids.FindAsync(id);
        if (aminoAcid == null) return NotFound();

        _context.AminoAcids.Remove(aminoAcid);
        await _context.SaveChangesAsync();

        return Ok();
    }
    //[HttpGet("get-food-ingredients")]
    //public async Task<IActionResult> GetFoodIngredients(string foodItem, string nType)
    //{
    //    var parameters = new[]
    //    {
    //    new SqlParameter("@Food_Item", foodItem ?? (object)DBNull.Value),
    //    new SqlParameter("@NType", nType ?? (object)DBNull.Value)
    //};

    //    if (nType == "V")
    //    {
    //        var result = await _context
    //            .Set<VitaminDto>()
    //            .FromSqlRaw("EXEC GetFood_ingredient @Food_Item, @NType", parameters)
    //            .ToListAsync();

    //        return Ok(result);
    //    }
    //    else if (nType == "M")
    //    {
    //        var result = await _context
    //            .Set<MineralDto>()
    //            .FromSqlRaw("EXEC GetFood_ingredient @Food_Item, @NType", parameters)
    //            .ToListAsync();

    //        return Ok(result);
    //    }

    //    return BadRequest("Invalid NType");
    //}

    //[HttpGet("get-food-ingredients")]
    //public async Task<IActionResult> GetFoodIngredients(string foodItem, string nType)
    //{
    //    var connection = _context.Database.GetDbConnection();
    //    await connection.OpenAsync();

    //    using (var command = connection.CreateCommand())
    //    {
    //        command.CommandText = "GetFood_ingredient";
    //        command.CommandType = CommandType.StoredProcedure;

    //        var foodItemParam = new SqlParameter("@Food_Item", SqlDbType.NVarChar, 100)
    //        {
    //            Value = (object)foodItem ?? DBNull.Value
    //        };
    //        var nTypeParam = new SqlParameter("@NType", SqlDbType.Char, 1)
    //        {
    //            Value = (object)nType ?? DBNull.Value
    //        };

    //        command.Parameters.Add(foodItemParam);
    //        command.Parameters.Add(nTypeParam);

    //        var reader = await command.ExecuteReaderAsync();
    //        var results = new List<Dictionary<string, object>>();

    //        while (await reader.ReadAsync())
    //        {
    //            var row = new Dictionary<string, object>();
    //            for (var i = 0; i < reader.FieldCount; i++)
    //            {
    //                row[reader.GetName(i)] = reader.GetValue(i);
    //            }
    //            results.Add(row);
    //        }

    //        return Ok(results);
    //    }
    //}
    //[HttpGet("get-food-ingredients")]
    //public async Task<IActionResult> GetFoodIngredients(string foodItem)
    //{
    //    var parameters = new[]
    //    {
    //    new SqlParameter("@Food_Item", foodItem ?? (object)DBNull.Value)
    //};

    //    using var command = _context.Database.GetDbConnection().CreateCommand();
    //    command.CommandText = "EXEC GetFood_ingredient @Food_Item";
    //    command.CommandType = System.Data.CommandType.Text;
    //    command.Parameters.AddRange(parameters);

    //    await _context.Database.OpenConnectionAsync();

    //    var results = new List<object>();
    //    using var reader = await command.ExecuteReaderAsync();

    //    do
    //    {
    //        var table = new DataTable();
    //        table.Load(reader);

    //        // Convert DataTable to a List of Dictionary<string, object>
    //        var tableData = table.AsEnumerable()
    //            .Select(row => table.Columns.Cast<DataColumn>()
    //                .ToDictionary(col => col.ColumnName, col => row[col]));

    //        results.Add(tableData);
    //    } while (!reader.IsClosed);

    //    await _context.Database.CloseConnectionAsync();

    //    return Ok(results);
    //}

    [HttpGet("get-food-ingredients-by-id")]
    public async Task<IActionResult> GetFoodIngredientsById(string sourceId)
    {
        if (string.IsNullOrEmpty(sourceId))
        {
            return BadRequest("SourceID is required.");
        }

        var parameters = new[]
        {
        new SqlParameter("@SourceID", sourceId)
    };

        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = "EXEC GetFood_ingredient @SourceID";
        command.CommandType = System.Data.CommandType.Text;
        command.Parameters.AddRange(parameters);

        await _context.Database.OpenConnectionAsync();

        var results = new List<object>();
        using var reader = await command.ExecuteReaderAsync();

        do
        {
            var table = new DataTable();
            table.Load(reader);

            // Convert DataTable to a List of Dictionary<string, object>
            var tableData = table.AsEnumerable()
                .Select(row => table.Columns.Cast<DataColumn>()
                    .ToDictionary(col => col.ColumnName, col => row[col]));

            results.Add(tableData);
        } while (!reader.IsClosed);

        await _context.Database.CloseConnectionAsync();

        return Ok(results);
    }

    [HttpGet("search-food-items")]
    public async Task<IActionResult> SearchFoodItems(string foodItem)
    {
        var parameters = new[]
        {
        new SqlParameter("@foodItem", foodItem ?? (object)DBNull.Value)
    };

        var result = await _context
            .FoodItemSearchResults // Replace with the mapped model or DTO
            .FromSqlRaw("EXEC GetfoodItem @foodItem", parameters)
            .ToListAsync();

        return Ok(result);
    }

    // Get top 10 foods by nutrient
    //[HttpGet("get-top-foods")]
    //public async Task<IActionResult> GetTopFoods([FromQuery] string nutrient)
    //{
    //    if (string.IsNullOrEmpty(nutrient))
    //    {
    //        return BadRequest("Nutrient parameter is required.");
    //    }

    //    var parameters = new[]
    //    {
    //        new SqlParameter("@Nutrient", nutrient)
    //    };

    //    var result = await _context.TopNutrients
    //        .FromSqlRaw("EXEC GetTop10FoodsByNutrient @Nutrient", parameters)
    //        .ToListAsync();

    //    return Ok(result);
    //}
    [HttpGet("get-top-foods")]
    public async Task<IActionResult> GetTopFoods(string nutrient)
    {
        if (string.IsNullOrEmpty(nutrient))
            return BadRequest("Nutrient parameter is required.");

        var parameters = new[]
        {
        new SqlParameter("@Nutrient", nutrient)
    };

        var result = await _context.Tran_TopNutrients
            .FromSqlRaw("EXEC GetTop10FoodsByNutrient @Nutrient", parameters)
            .ToListAsync();

        return Ok(result);
    }

    // Insert a new record
    [HttpPost("insert-top-nutrient")]
    public async Task<IActionResult> InsertTopNutrient([FromBody] TopNutrient model)
    {
        if (model == null)
        {
            return BadRequest("Invalid data.");
        }

        var parameters = new[]
        {
            new SqlParameter("@FoodItem", model.FoodItem ?? (object)DBNull.Value),
            new SqlParameter("@Ntype", model.Ntype ?? (object)DBNull.Value),
            new SqlParameter("@Nutritions", model.Nutritions ?? (object)DBNull.Value),
            new SqlParameter("@NVale", model.NVale)
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC InsertTopNutrient @FoodItem, @Ntype, @Nutritions, @NVale", parameters);

        return Ok("Record added successfully.");
    }

    // Update an existing record
    [HttpPut("update-top-nutrient")]
    public async Task<IActionResult> UpdateTopNutrient([FromBody] TopNutrient model)
    {
        if (model == null || model.Id <= 0)
        {
            return BadRequest("Invalid data.");
        }

        var parameters = new[]
        {
            new SqlParameter("@Id", model.Id),
            new SqlParameter("@FoodItem", model.FoodItem ?? (object)DBNull.Value),
            new SqlParameter("@Ntype", model.Ntype ?? (object)DBNull.Value),
            new SqlParameter("@Nutritions", model.Nutritions ?? (object)DBNull.Value),
            new SqlParameter("@NVale", model.NVale)
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC UpdateTopNutrient @Id, @FoodItem, @Ntype, @Nutritions, @NVale", parameters);

        return Ok("Record updated successfully.");
    }

    // Delete a record
    [HttpDelete("delete-top-nutrient")]
    public async Task<IActionResult> DeleteTopNutrient([FromQuery] int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid record ID.");
        }

        var parameters = new[]
        {
            new SqlParameter("@Id", id)
        };

        await _context.Database.ExecuteSqlRawAsync("EXEC DeleteTopNutrient @Id", parameters);

        return Ok("Record deleted successfully.");
    }

    // [HttpGet("top-protein-sources")]
    //public async Task<IActionResult> GetTopProteinSources()
    //{
    //    var tempResults = await _context.Tran_AminoAcids
    //        .Select(a => new
    //        {
    //            FoodItem = a.FoodItem,
    //            Histidine = a.HISTIDINE,
    //            Lysine = a.LYSINE,
    //            Isoleucine = a.ISOLEUCINE,
    //            Leucine = a.LEUCINE,
    //            Theronine = a.THERONINE,
    //            Methionine = a.METHIONINE,
    //            Phenylalanine = a.PHENYLALANINE,
    //            Trytophan = a.TRYTOPHAN,
    //            TotalAminoAcids = a.HISTIDINE + a.LYSINE + a.ISOLEUCINE + a.LEUCINE +
    //                               a.THERONINE + a.METHIONINE + a.PHENYLALANINE + a.TRYTOPHAN
    //        })
    //        .ToListAsync();

    //    var results = tempResults
    //        .GroupBy(a => a.FoodItem)
    //        .Select(g => g.OrderByDescending(a => a.TotalAminoAcids).FirstOrDefault())
    //        .OrderByDescending(a => a.TotalAminoAcids)
    //        .Take(10)
    //        .Select(a => new
    //        {
    //            foodItem = a.FoodItem,
    //            histidine = a.Histidine,
    //            lysine = a.Lysine,
    //            isoleucine = a.Isoleucine,
    //            leucine = a.Leucine,
    //            theronine = a.Theronine,
    //            methionine = a.Methionine,
    //            phenylalanine = a.Phenylalanine,
    //            trytophan = a.Trytophan
    //        })
    //        .ToList(); // materialise to list.

    //    return Ok(results);
    //}
    //[HttpGet("top-protein-sources")]
    //public async Task<IActionResult> GetTopProteinSources()
    //{
    //    var results = await _context.Set<ProteinSourceDto>()
    //        .FromSqlRaw("EXEC GetTop10ProteinSources")
    //        .ToListAsync();

    //    return Ok(results);
    //}

    
    [HttpGet("top-protein-sources")]
    public async Task<IActionResult> GetTopProteinSources()
    {
        var result = new List<List<Dictionary<string, object>>>();

        var conn = _context.Database.GetDbConnection();
        await conn.OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "GetTop10ProteinSources";
        cmd.CommandType = CommandType.StoredProcedure;

        using var reader = await cmd.ExecuteReaderAsync();

        do
        {
            var table = new List<Dictionary<string, object>>();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? 0 : reader.GetValue(i);
                }
                table.Add(row);
            }
            result.Add(table);
        } while (await reader.NextResultAsync());

        await conn.CloseAsync();

        return Ok(result);
    }

    [HttpGet("top-carbohydrate-sources")]
    public async Task<IActionResult> GetTopCarbohydrateSources()
    {
        var result = new List<List<Dictionary<string, object>>>();

        var conn = _context.Database.GetDbConnection();
        await conn.OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "GetTop10CarbohydrateSources";
        cmd.CommandType = CommandType.StoredProcedure;

        using var reader = await cmd.ExecuteReaderAsync();

        do
        {
            var table = new List<Dictionary<string, object>>();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? 0 : reader.GetValue(i);
                }
                table.Add(row);
            }
            result.Add(table);
        } while (await reader.NextResultAsync());

        await conn.CloseAsync();

        return Ok(result);
    }

    [HttpGet("top-fat-sources")]
    public async Task<IActionResult> GetTopFatSources()
    {
        var result = new List<List<Dictionary<string, object>>>();

        var conn = _context.Database.GetDbConnection();
        await conn.OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "GetTop10FatSources";
        cmd.CommandType = CommandType.StoredProcedure;

        using var reader = await cmd.ExecuteReaderAsync();

        do
        {
            var table = new List<Dictionary<string, object>>();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? 0 : reader.GetValue(i);
                }
                table.Add(row);
            }
            result.Add(table);
        } while (await reader.NextResultAsync());

        await conn.CloseAsync();

        return Ok(result);
    }
    [HttpPost("log-visit")]
    public async Task<IActionResult> LogVisit()
    {
        // Extract IP from header or fallback to remote address
        var ipRaw = Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                    HttpContext.Connection.RemoteIpAddress?.ToString();

        // Remove port if present (e.g., "86.17.88.196:52780")
        var ip = ipRaw?.Split(':').FirstOrDefault();

        var userAgent = Request.Headers["User-Agent"].ToString();
        var url = Request.Headers["X-Page-Url"].ToString();
        var referrer = Request.Headers["Referer"].ToString();

        string country = "", region = "", city = "";

        if (string.IsNullOrEmpty(ip) || ip == "::1" || ip.StartsWith("127.") || ip.StartsWith("::ffff:127."))
        {
            country = "Localhost";
            region = "Development";
            city = "Local";
        }
        else
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "NutritionApp");

                    var apiUrl = $"https://ipapi.co/{ip}/json/";
                    var locationResponse = await httpClient.GetStringAsync(apiUrl);
                    Console.WriteLine($"GeoLookup Response: {locationResponse}");

                    dynamic locationData = JsonConvert.DeserializeObject(locationResponse);
                    country = locationData?.country_name ?? "";
                    region = locationData?.region ?? "";
                    city = locationData?.city ?? "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"GeoLookup failed for IP {ip}: {ex.Message}");
                }
            }

        }

        var log = new VisitorLog
        {
            IPAddress = ip,
            UserAgent = userAgent,
            Country = country,
            Region = region,
            City = city,
            UrlVisited = url,
            Referrer = referrer,
            VisitTime = DateTime.UtcNow
        };

        _context.VisitorLogs.Add(log);
        await _context.SaveChangesAsync();

        return Ok();
    }



    [HttpGet("visitor-logs")]
    public async Task<IActionResult> GetVisitorLogs()
    {
        var logs = await _context.VisitorLogs
            .OrderByDescending(x => x.VisitTime)
            .Take(100) // Latest 100
            .ToListAsync();

        return Ok(logs);
    }
}
