using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Identity.BindingModels.Authentication; 

public class RegisterPostModel {
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}