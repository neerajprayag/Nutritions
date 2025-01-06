using System.ComponentModel.DataAnnotations;

namespace Nutritions.Model
{
    public class Meals
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MealName { get; set; }
        public string Calories { get; set; }
        public string Protein { get; set; }
        public string Carbs { get; set; }
        public string Fat { get; set; }
        public string Category { get; set; }
        public DateTime MealDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
