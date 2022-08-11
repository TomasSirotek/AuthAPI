using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Identity.BindingModels; 

public class UserPostModel {
    [Required]
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    [Required]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string Email { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 4)]
    
    public List<string> Roles { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    public bool IsActivated { get; set; }
    
}