using System.ComponentModel.DataAnnotations;


namespace WebApp1.Models.DataTransferObjects
{
    public class Dto_Product
    {

        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool isPopular { get; set; } = false;
    }
}

