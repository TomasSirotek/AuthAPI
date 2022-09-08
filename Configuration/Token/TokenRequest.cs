using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Domain.BindingModels;

public class TokenRequest {
    [Required] 
    public string Token { get; set; }
    [Required] 
    public string RefreshToken { get; set; }
}