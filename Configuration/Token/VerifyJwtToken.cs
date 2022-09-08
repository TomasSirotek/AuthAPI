using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AuthAPI.Domain.BindingModels;
using AuthAPI.Domain.Models;
using AuthAPI.Engines;
using AuthAPI.Identity.Models;
using AuthAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AuthAPI.Configuration.Token; 

public class VerifyJwtToken : IVerifyJwtToken {
    
    private readonly IUnixStampDateConverter _unixHelper;
    private readonly TokenValidationParameters _tokenValidationParams;
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    private readonly IJwtToken _token;
    public VerifyJwtToken(IUnixStampDateConverter unixHelper, TokenValidationParameters tokenValidationParams, IConfiguration config, IUserService userService, IJwtToken token)
    {
        _unixHelper = unixHelper;
        _tokenValidationParams = tokenValidationParams;
        _config = config;
        _userService = userService;
        _token = token;
    }

    public  async Task<TokenResponse> VerifyAndGenerateToken(TokenRequest tokenRequest)
    {
       var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("JwtConfig:Secret").Value));
       
       var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                _tokenValidationParams.ValidateLifetime = false;
                _tokenValidationParams.IssuerSigningKey = key;
                _tokenValidationParams.ValidateAudience = false;
                _tokenValidationParams.ValidateIssuer = false;

                jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
                        StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                        throw new Exception("Could not validate token");
                }

                var jwtToken = (JwtSecurityToken) validatedToken;

                var value = jwtToken.Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)
                    ?.Value;
                if (value != null)
                {
                    var utcExpiryDate = long.Parse(value);

                    var expiryDate = _unixHelper.UnixTimeStampToDateTime(utcExpiryDate);

                    if (expiryDate > DateTime.Now.AddDays(7))
                    {
                        throw new Exception("Token is expired");
                    }
                }
                
                RefreshToken storedToken = await _userService.FindTokenAsync(tokenRequest.RefreshToken);
                AppUser user = await _userService.GetUserByIdAsync(storedToken.UserId);
                var token = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id);
                // generate new JWT for now with 15 min exp date // 
                //TODO: Needs to be refactored and change to have longer life 
                TokenResponse response = new TokenResponse()
                {
                    Token = token
                };
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
    }
}
