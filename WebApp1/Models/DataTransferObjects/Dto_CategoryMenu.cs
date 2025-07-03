using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models.DataTransferObjects
{
    public class Dto_CategoryMenu
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı gereklidir.")]
        public string Name { get; set; } = "";

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}