namespace ProductAPI.Domain.Models {
    public class Category {

        public string Id { get; set; }
        
        public string Platform { get; set; }
        
        public Category(string id, string platform)
        {
            Id = id;
            Platform = platform;
        }

        public Category() {}
    }
}