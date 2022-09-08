using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Identity.BindingModels.Authentication; 

public class AuthPostModel {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}