using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Identity.BindingModels; 

public class UserPostModel {
    [Required]
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    [Required]
    public string Email { get; set; }
    [Required]
    public List<string> Roles { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    public bool IsActivated { get; set; }
    
}