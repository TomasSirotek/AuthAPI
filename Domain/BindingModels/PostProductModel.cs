using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Domain.BindingModels {
    public class PostProductModel {
        [Required]
        public string Title { get; set; } 
        [Required]
        public bool Price { get; set; } 
        [Required]
        public string Description { get; set; } 
    
        public List<string> Category { get; set; } 
    
        public bool IsAvailable { get; set; } 
    
        public string AgeLimit { get; set; } 

    }
}