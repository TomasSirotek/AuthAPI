using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProductAPI.Configuration.Token;

public class JwtToken : IJwtToken {
    private readonly IConfiguration _config;

    public JwtToken(IConfiguration config)
    {
        _config = config;
    }

    public string CreateToken(List<string> roles, string userId, double duration)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, userId),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
        };

        foreach (var i in roles)
            claims.Add(new Claim(ClaimTypes.Role, i));


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config.GetSection("JwtConfig:Secret").Value));
            
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(duration), // AddMinutes not seconds
            signingCredentials: credentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
    // public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    // {
    //     var tokenValidationParameters = new TokenValidationParameters
    //     {
    //         ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
    //         ValidateIssuer = false,
    //         ValidateIssuerSigningKey = true,
    //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes( _config.GetSection("JwtConfig:Secret").Value)),
    //         ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
    //     };
    //
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     SecurityToken securityToken;
    //     var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
    //     var jwtSecurityToken = securityToken as JwtSecurityToken;
    //     if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
    //         throw new SecurityTokenException("Invalid token");
    //
    //     return principal;
    // }
    
}