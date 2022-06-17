using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Domain.BindingModels {
    public class PutCharacterModel {
        [Required]
        public string Id { get; set; } 

        public string FullName { get; set; } 

        public bool Status { get; set; } 

        public string KnownFor { get; set; } 
    
        public string Gender { get; set; } 
    
        public bool IsAvailable { get; set; } 
    
        public string Nationality { get; set; } 
    }
}