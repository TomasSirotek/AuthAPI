namespace ProductAPI.Domain.Models {
    public class Category {

        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public Category(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public Category() {}
    }
}