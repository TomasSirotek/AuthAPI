using System.ComponentModel.DataAnnotations;
using AuthAPI.Domain.BindingModels;
using AuthAPI.Domain.Models;


namespace AuthAPI.Identity.BindingModels; 

public class UserPostModel {
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public List<string> Roles { get; set; }
    public string Password { get; set; }
    
    public AddressPostModel Address { get; set; }
    [Required]
    public bool IsActivated { get; set; }
    
}