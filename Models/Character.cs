using System.ComponentModel.DataAnnotations;

namespace Data_Access.Models {
    public class Character {
        
        [Key]
        [MaxLength(255)]
        public string Id { get; set; } 
    
        public string FullName { get; set; } 
    
        public bool Status { get; set; } 
    
        public string KnownFor { get; set; } 
    
        public string Gender { get; set; } 
    
        public DateTime DoB { get; set; } 
    
        public string Nationality { get; set; }

        public Character(string id, string fullName,bool status,string knownFor,string gender, DateTime dob,string nationality) {
            Id = id;
            FullName = fullName;
            Status = status;
            KnownFor = knownFor;
            Gender = gender;
            DoB = dob;
            Nationality = nationality;
        }
        
    public Character(){}
    
    }
}