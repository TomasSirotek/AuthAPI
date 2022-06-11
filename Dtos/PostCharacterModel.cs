using System.ComponentModel.DataAnnotations;

namespace Data_Access.Dtos {
    public class PostCharacterModel {
        [Key]
        public string Id { get; set; } 
        [Required]
        public string FullName { get; set; } 
        [Required]
        public bool Status { get; set; } 
        [Required]
        public string KnownFor { get; set; } 
    
        public string Gender { get; set; } 
    
        public DateTime DoB { get; set; } 
    
        public string Nationality { get; set; } 

    }
}