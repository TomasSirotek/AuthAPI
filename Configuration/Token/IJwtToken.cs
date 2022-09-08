using ProductAPI.Domain.Models;

namespace ProductAPI.Configuration.Token;

public interface IJwtToken {
    public string CreateToken(List<string> roles, string userId);
    
    public Task<RefreshToken> GenerateRefreshToken(string userId);
}