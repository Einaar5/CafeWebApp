using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models
{
    public class CategoryMenu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property - One Category to Many Products
        public ICollection<Product> Products { get; set; }

    }
}
