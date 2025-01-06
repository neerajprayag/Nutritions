using System.ComponentModel.DataAnnotations;

namespace Nutritions.Model
{
    public class MealItems
    {
        [Key]
        public int Id { get; set; }
        public int MealId { get; set; }
        public string FoodItem { get; set; }
        public string FoodWeight { get; set; }
        public string Calories { get; set; }
        public string Protein { get; set; }
        public string Carbs { get; set; }
        public string Fat { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
