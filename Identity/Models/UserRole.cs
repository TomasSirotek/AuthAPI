namespace ProductAPI.Identity.Models {
    public class UserRole {
    
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public UserRole(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public UserRole() {}
    }
}

