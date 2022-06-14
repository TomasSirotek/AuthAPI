using System.ComponentModel.DataAnnotations;

namespace Data_Access.Models {
    public class Category {
        
        [Key]
        [MaxLength(255)]
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