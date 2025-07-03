using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models
{
    public class Content
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Logo Kısmını Girin")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(10, ErrorMessage = "Başında 0 Olmadan Telefon Numarasını Girin")]
        public string PhoneNumber { get; set; } = "";
        public string HeaderTitleSm { get; set; } = "";
        public string HeaderTitleGr { get; set; } = "";

        public string HeaderImageUrl1 { get; set; } = "";
        public string HeaderImageUrl2 { get; set; } = "";

        public string HeaderParagraph { get; set; } = "";

        public string AboutTitle { get; set; } = "";

        public string AboutParagraph { get; set; } = "";

        public string AboutImageUrl { get; set; } = "";

        public string MenuTitle { get; set; } = "";

        public string GalleryTitle { get; set; } = "";

        public string FooterTitle { get; set; } = "";

        public string FooterFaceBookUrl { get; set; } = "";

        public string FooterInstagramUrl { get; set; } = "";

        public string FooterTwitterUrl { get; set; } = "";

        public string FooterLocation { get; set; } = "";

        public string FooterOpeningHours { get; set; } = "";

        public string FooterEmail { get; set; } = "";








    }
}
