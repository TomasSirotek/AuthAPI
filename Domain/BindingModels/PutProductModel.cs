using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Domain.BindingModels {
    public class PutProductModel {
        [Required]
        public string Id { get; set; }
      
        public string Title { get; set; } 
     
        public string Price { get; set; }
        
        public string Description { get; set; } 
    
        public List<string> Category { get; set; } 
    
        public bool IsAvailable { get; set; } 
    
        public string AgeLimit { get; set; } 
    }
}