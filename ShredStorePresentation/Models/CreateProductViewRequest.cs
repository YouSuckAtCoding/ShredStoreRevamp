using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShredStorePresentation.Models
{
    public class CreateProductViewRequest
    {
        
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please insert a name")]
        public string Name { get; set; }
        [Required]
        [MaxLength(750, ErrorMessage = "Descriptionmus have between 30 and 300 characters.")]
        [MinLength(30, ErrorMessage = "Descriptionmus have between 30 and 300 characters.")]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Brand { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        [Display(Name = "Product Image")]
        public IFormFile ImageFile { get; set; }
    }
}
