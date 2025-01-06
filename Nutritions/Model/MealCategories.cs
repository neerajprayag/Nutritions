using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Nutritions.Model
{
    public class MealCategories
    {
        [Key]
        public int Id { get; set; }
        public string MealCategroyName { get; set; }
    }
}
