using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nutritions.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Nutritions.Controllers
{
    [Route("api/meal-tracker")]
    [ApiController]
    public class MealTrackerController : ControllerBase
    {
        private readonly NutritionDbContext _context;

        public MealTrackerController(NutritionDbContext context)
        {
            _context = context;
        }

        // Add Meal
        [HttpPost("add-meal")]
        public async Task<IActionResult> AddMeal([FromBody] MealDto mealDto)
        {
            try
            {
                // Insert into Meals table and retrieve the generated MealId
                var parameters = new[]
                {
            new SqlParameter("@UserId", mealDto.UserId),
            new SqlParameter("@MealName", mealDto.MealName),
            new SqlParameter("@Calories", mealDto.Calories),
            new SqlParameter("@Protein", mealDto.Protein),
            new SqlParameter("@Carbs", mealDto.Carbs),
            new SqlParameter("@Fat", mealDto.Fat),
            new SqlParameter("@Category", mealDto.CategoryId),
            new SqlParameter("@MealDate", mealDto.MealDate),
        };

                var mealIdParameter = new SqlParameter
                {
                    ParameterName = "@MealId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var sql = "EXEC AddMeal @UserId, @MealName, @Calories, @Protein, @Carbs, @Fat, @Category, @MealDate, @MealId OUTPUT";

                await _context.Database.ExecuteSqlRawAsync(sql, parameters.Concat(new[] { mealIdParameter }).ToArray());
                var mealId = (int)mealIdParameter.Value;

                // Insert related MealItems
                foreach (var item in mealDto.MealItems)
                {
                    var itemParameters = new[]
                    {
                new SqlParameter("@MealId", mealId),
                new SqlParameter("@FoodItem", item.FoodItem),
                new SqlParameter("@FoodWeight", item.FoodWeight),
                new SqlParameter("@Calories", item.Calories),
                new SqlParameter("@Protein", item.Protein),
                new SqlParameter("@Carbs", item.Carbs),
                new SqlParameter("@Fat", item.Fat),
                new SqlParameter("@CreatedDate", DateTime.UtcNow),
                new SqlParameter("@ModifiedDated", DBNull.Value),
                new SqlParameter("@IsDeleted", false),
            };

                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC AddMealItem @MealId, @FoodItem, @FoodWeight, @Calories, @Protein, @Carbs, @Fat, @CreatedDate, @ModifiedDated, @IsDeleted",
                        itemParameters);
                }

                return Ok("Meal added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // Fetch Meal Categories
        [HttpGet("meal-categories")]
        public async Task<IActionResult> GetMealCategories()
        {
            try
            {
                var result = await _context.MealCategories
                    .FromSqlRaw("EXEC GetMealCategoryList")
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("meal-history/{userId}")]
        public async Task<IActionResult> GetMealHistory(int userId)
        {
            try
            {
                var parameter = new SqlParameter("@UserId", userId);
                var meals = await _context.Meals
                    .FromSqlRaw("EXEC GetMealHistory @UserId", parameter)
                    .ToListAsync();
                return Ok(meals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("meal-details/{mealId}")]
        public async Task<IActionResult> GetMealDetails(int mealId)
        {
            try
            {
                var parameter = new SqlParameter("@MealId", mealId);
                var mealDetails = await _context.MealItems
                    .FromSqlRaw("EXEC GetMealDetails @MealId", parameter)
                    .ToListAsync();
                return Ok(mealDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-meal/{mealId}")]
        public async Task<IActionResult> UpdateMeal(int mealId, [FromBody] MealDto mealDto)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@MealId", mealId),
            new SqlParameter("@MealName", mealDto.MealName),
            new SqlParameter("@Category", mealDto.CategoryId),
            new SqlParameter("@Calories", mealDto.Calories),
            new SqlParameter("@Protein", mealDto.Protein),
            new SqlParameter("@Carbs", mealDto.Carbs),
            new SqlParameter("@Fat", mealDto.Fat),
        };
                await _context.Database.ExecuteSqlRawAsync("EXEC UpdateMeal @MealId, @MealName, @Category, @Calories, @Protein, @Carbs, @Fat", parameters);
                return Ok("Meal updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete-meal/{mealId}")]
        public async Task<IActionResult> DeleteMeal(int mealId)
        {
            try
            {
                var parameter = new SqlParameter("@MealId", mealId);
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteMeal @MealId", parameter);
                return Ok("Meal deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // Get Suggestions
        [HttpGet("suggest-meal")]
        public async Task<IActionResult> SuggestMeal([FromQuery] double weight, [FromQuery] double height)
        {
            var parameters = new[]
            {
                new SqlParameter("@Weight", weight),
                new SqlParameter("@Height", height)
            };

            var suggestions = await _context.Tran_TopNutrients
                .FromSqlRaw("EXEC SuggestMeal @Weight, @Height", parameters)
                .ToListAsync();

            return Ok(suggestions);
        }

        [HttpGet("search-food-item")]
        public async Task<IActionResult> SearchFoodItem([FromQuery] string foodItem)
        {
            if (string.IsNullOrEmpty(foodItem))
                return BadRequest("Food item is required.");

            var parameters = new[] { new SqlParameter("@FoodItem", foodItem) };

            var result = await _context.TopNutrientFoods
                .FromSqlRaw("EXEC GetFoodNutrients @FoodItem", parameters)
                .ToListAsync();

            return Ok(result);
        }

    }

    // DTO for Meal
    public class MealDto
    {
        public int UserId { get; set; }
        public string MealName { get; set; } // Matches "MealName" in JSON
       
        public string Calories { get; set; } // Matches "Calories" in JSON
        public string Protein { get; set; }  // Matches "Protein" in JSON
        public string Carbs { get; set; }    // Matches "Carbs" in JSON
        public string Fat { get; set; }      // Matches "Fat" in JSON
        public int CategoryId { get; set; }  // Matches "CategoryId" in JSON
        public DateTime MealDate { get; set; } // Matches "MealDate" in JSON
        public List<MealItemDto> MealItems { get; set; } // Matches "MealItems" in JSON
    }

    public class MealItemDto
    {
        public string FoodItem { get; set; } // Matches "FoodItem" in JSON
        public string FoodWeight { get; set; } // Matches "FoodWeight" in JSON
        public string Calories { get; set; } // Matches "Calories" in JSON
        public string Protein { get; set; } // Matches "Protein" in JSON
        public string Carbs { get; set; } // Matches "Carbs" in JSON
        public string Fat { get; set; } // Matches "Fat" in JSON
    }


}
