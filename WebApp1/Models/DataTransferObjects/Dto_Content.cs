using System.ComponentModel.DataAnnotations;

namespace WebApp1.Models.DataTransferObjects
{
    public class Dto_Content
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title must be at most 100 characters")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(10, ErrorMessage = "Enter phone number without 0 at the beginning")]
        public string PhoneNumber { get; set; } = "";

        public string HeaderTitleSm { get; set; } = "";
        public string HeaderTitleGr { get; set; } = "";

        public IFormFile HeaderImageUrl1 { get; set; } // Image upload
        public IFormFile HeaderImageUrl2 { get; set; } // Image upload
        public string HeaderParagraph { get; set; } = "";

        public string AboutTitle { get; set; } = "";
        public string AboutParagraph { get; set; } = "";
        public IFormFile AboutImageUrl { get; set; } // Image upload

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