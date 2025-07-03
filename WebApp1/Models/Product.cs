using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        
        [ForeignKey("CategoryMenu")]
        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        // Navigation Property
        public CategoryMenu Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ImageUrl { get; set; } = "";

        public bool isPopular { get; set; } = false;
    }
}
