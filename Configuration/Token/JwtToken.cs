using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthAPI.Domain.Models;
using AuthAPI.Infrastructure.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AuthAPI.Configuration.Token;

public class JwtToken : IJwtToken {
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    public JwtToken(IConfiguration config, IUserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
    }

    public string CreateToken(List<string> roles, string userId)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, userId)
        };

        foreach (var i in roles)
            claims.Add(new Claim(ClaimTypes.Role, i));


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config.GetSection("JwtConfig:Secret").Value));
            
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires:DateTime.UtcNow.Add(TimeSpan.Parse(_config.GetSection("JwtConfig:ExpiryTimeFrame").Value)),
            signingCredentials: credentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public async Task<RefreshToken> GenerateRefreshToken(string usedId)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid().ToString(),
            UserId = usedId,
            Token = GetUniqueToken(),
            IsUsed = false,
            IsRevoked = false,
            ExpDate = DateTime.UtcNow.AddDays(7),
            AddedDate = DateTime.UtcNow,
        };
        bool savedToken = await _userRepository.UpdateToken(refreshToken);
        if (!savedToken) 
            throw new Exception("Could not save token");
        
        return refreshToken;
        
        string GetUniqueToken()
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var tokenIsUnique = true;

            if (!tokenIsUnique)
                return GetUniqueToken();
            return token;
        }
    }

    
}