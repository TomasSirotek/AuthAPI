namespace ProductAPI.Domain.Models {
    public class Product {

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public bool IsActive { get; set; }

        public decimal UnitPrice { get; set; }

        public int? UnitsInStock { get; set; }

        public Product(string id, string title, string description, string image, bool isActive, decimal unitPrice,
            int? unitsInStock)
        {
            Id = id;
            Title = title;
            Description = description;
            Image = image;
            IsActive = isActive;
            UnitPrice = unitPrice;
            UnitsInStock = unitsInStock;
            
        }
        public bool IsAvailable(int quantity)
        {
            return IsActive && UnitsInStock >= quantity;
        }
        public Product() {}
    }
}