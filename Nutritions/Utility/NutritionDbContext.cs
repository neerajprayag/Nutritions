using Microsoft.EntityFrameworkCore;
using Nutritions.Model;

namespace Nutritions.Utility
{
    public class NutritionDbContext : DbContext
    {
        public NutritionDbContext(DbContextOptions<NutritionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vitamin> Vitamins { get; set; }
        public DbSet<Mineral> Minerals { get; set; }
        public DbSet<AminoAcids> AminoAcids { get; set; }
        public DbSet<FoodIngredient> FoodIngredients { get; set; }
        public DbSet<VitaminDto> VitaminDto { get; set; }
        public DbSet<MineralDto> MineralDto { get; set; }
        public DbSet<FoodItemSearchResults> FoodItemSearchResults { get; set; }
        public DbSet<TopNutrient> Tran_TopNutrients { get; set; }
        public DbSet<Meals> Meals { get; set; }
        public DbSet<MealItems> MealItems { get; set; }
        public DbSet<TopNutrientFoods> TopNutrientFoods { get; set; }
        public DbSet<MealCategories> MealCategories { get; set; }
     


        //public DbSet<SuperFood> SuperFoods { get; set; }
        //public DbSet<NutrientSource> NutrientSources { get; set; }
    }
}
