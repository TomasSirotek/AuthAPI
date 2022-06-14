using System.ComponentModel.DataAnnotations;

namespace Data_Access.Models {
    public class Product {
        
        [Key]
        [MaxLength(255)]
        public string Id { get; set; } 
    
        public string Title { get; set; } 
    
        public int Price { get; set; } 
    
        public string Description { get; set; } 
    
        public List<Category> Category { get; set; } 
    
        public DateTime DoB { get; set; } 
    
        public string AgeLimit { get; set; } 
        
        
        public Product(string id, string title,int price,string description, DateTime dob,string ageLimit) {
            Id = id;
            Title = title;
            Price = price;
            Description = description;
            DoB = dob;
            AgeLimit = ageLimit;
        }
        
    public Product()
    {
    }
    }
}