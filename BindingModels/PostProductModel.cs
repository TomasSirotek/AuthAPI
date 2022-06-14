using System.ComponentModel.DataAnnotations;

namespace Data_Access.BindingModels {
    public class PostProductModel {
        [Required]
        public string Title { get; set; } 
        [Required]
        public bool Price { get; set; } 
        [Required]
        public string Description { get; set; } 
    
        public List<string> Category { get; set; } 
    
        public DateTime DoB { get; set; } 
    
        public string AgeLimit { get; set; } 

    }
}