using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShredStorePresentation.Models
{
    public class UpdateProductViewRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        [Display(Name = "Product Image")]
        public IFormFile ImageFile { get; set; }
    }
}
