using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Domain.BindingModels {
    public class PutProductModel {
        [Required]
        public string Id { get; set; }
      
        public string Title { get; set; } 
        
        public string Description { get; set; } 
        
        public string Image { get; set; } 
    
        public List<string> Category { get; set; } 
    
        public bool IsActive { get; set; }

        public decimal UnitPrice { get; set; }
        
        public int? UnitsInStock { get; set; }
    }
}