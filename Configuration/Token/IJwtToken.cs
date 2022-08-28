using ProductAPI.Domain.Models;

namespace ProductAPI.Configuration.Token;

public interface IJwtToken {
    public string CreateToken(List<string> roles, string userId);
    
    public string? ValidateJwtToken(string? token);
    public Task<RefreshToken> GenerateRefreshToken(string userId);
}