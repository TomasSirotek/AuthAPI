using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Identity.BindingModels {
    public class RolePostModel {
        [Required]
        public string Name { get; set; } 
    }
}