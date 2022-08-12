using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Domain.BindingModels.Category; 

public class PutCategoryModel {
    [Required]
    public string Id { get; set; }
    [EmailAddress]
    public string Name { get; set; } 
}