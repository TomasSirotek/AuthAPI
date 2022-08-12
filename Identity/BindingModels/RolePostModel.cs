using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Identity.BindingModels {
    public class RolePostModel {
        [Required]
        public string Name { get; set; } 
    }
}