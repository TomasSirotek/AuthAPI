using System.ComponentModel.DataAnnotations;

namespace Data_Access.Dtos {
    public class PutCharacterModel {
        [Required]
        [Key]
        public string Id { get; set; } 

        public string FullName { get; set; } 

        public bool Status { get; set; } 

        public string KnownFor { get; set; } 
    
        public string Gender { get; set; } 
    
        public DateTime DoB { get; set; } 
    
        public string Nationality { get; set; } 
    }
}