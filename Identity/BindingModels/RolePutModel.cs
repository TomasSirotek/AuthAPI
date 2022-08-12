using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Identity.BindingModels {
    public class RolePutModel {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; } 
    }
}