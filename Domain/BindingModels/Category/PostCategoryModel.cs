using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Domain.BindingModels.Category {
    public class PostCategoryModel {
        [Required]
        public string Name { get; set; } 
    }
}