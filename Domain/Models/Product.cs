namespace ProductAPI.Domain.Models {
    public class Product {
        
        public string Id { get; set; } 
    
        public string Title { get; set; } 
        
        public string Description { get; set; } 
        
        public string Image { get; set; } 
    
        public List<Category> Category { get; set; } 
        
        public string AgeLimit { get; set; } 
        
        public decimal UnitPrice { get; set; }
        
        public int? UnitsInStock { get; set; }
        
        public Product(string id, string title,string description,string image,string ageLimit,decimal unitPrice,int? unitsInStock) {
            Id = id;
            Title = title;
            Description = description;
            Image = image;
            AgeLimit = ageLimit;
            UnitPrice = unitPrice;
            UnitsInStock = unitsInStock;
        }
        
    public Product() {}
    }
}