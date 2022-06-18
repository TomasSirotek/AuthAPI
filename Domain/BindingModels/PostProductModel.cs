using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Domain.BindingModels {
    public class PostProductModel {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; } 
        [Required]
        public string Image { get; set; } 

        public List<string> Category { get; set; } 
        [Required]
        public bool IsActive { get; set; } 
 
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int? UnitsInStock { get; set; }

    }
}