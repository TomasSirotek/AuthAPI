using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;

namespace ProductAPI.Configuration.Token;

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
    public string? ValidateJwtToken(string? token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["JwtConfig:RefreshTokenSecret"]);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == "id").Value;
       
            return userId;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
            // ensure token is unique by checking against db
            var tokenIsUnique = true;

            if (!tokenIsUnique)
                return GetUniqueToken();
            return token;
        }
    }

    
}