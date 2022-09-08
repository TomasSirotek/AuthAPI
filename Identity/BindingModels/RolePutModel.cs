using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Identity.BindingModels {
    public class RolePutModel {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; } 
    }
}