namespace ProductAPI.Domain.Models {
    public class Product {
        
        public string Id { get; set; } 
    
        public string Title { get; set; } 
    
        public string Price { get; set; } 
    
        public string Description { get; set; } 
    
        public List<Category> Category { get; set; } 
    
        public bool IsAvailable { get; set; } 
    
        public string AgeLimit { get; set; } 
        
        
        public Product(string id, string title,string price,string description,bool isAvailable,string ageLimit) {
            Id = id;
            Title = title;
            Price = price;
            Description = description;
            IsAvailable = isAvailable;
            AgeLimit = ageLimit;
        }
        
    public Product()
    {
    }
    }
}