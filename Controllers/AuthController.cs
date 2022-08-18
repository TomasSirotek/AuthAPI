using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Configuration.Token;
using ProductAPI.Domain.Models;
using ProductAPI.Engines.Cryptography;
using ProductAPI.Identity;
using ProductAPI.Identity.BindingModels.Authentication;
using ProductAPI.Identity.Models;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers {
    
    public class AuthController : DefaultController{
        
        private readonly IJwtToken _token;
        private readonly IUserService _userService;
        private readonly ICryptoEngine _cryptoEngine;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly AppUserManager<AppUser> _userManager;
        
        
        public AuthController (IJwtToken token,IUserService userService,ICryptoEngine cryptoEngine,TokenValidationParameters tokenValidationParams, AppUserManager<AppUser> userManager)
        {
            _token = token;
            _userService = userService;
            _cryptoEngine = cryptoEngine;
            _tokenValidationParams = tokenValidationParams;
            _userManager = userManager;
        }

        [HttpPost ()] 
        public async Task<ActionResult> Authenticate ([FromBody]AuthPostModel request)
        {
            AppUser user = await _userService.GetAsyncByEmailAsync(request.Email);
            if (user == null)
                return BadRequest($"User email: {request.Email} is not correct!");

            if (!_cryptoEngine.HashCheck(user.PasswordHash, request.Password))
                return BadRequest("Password incorrect !");

            var token = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24); // 24
            user.Token = token;
          
            return Ok(user);
        }
	
        [HttpPost ("register")] 
        public async Task<IActionResult> Register ([FromBody]RegisterPostModel model,CancellationToken cancellationToken)
        {
		
            AppUser user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsActivated = true,
                CreatedAt = DateTime.Now
            };
            // Find by Email if exist !! 
            
            AppUser newUser = await _userManager.RegisterUserAsync(user, model.Password,cancellationToken);
            // get user with roles etc 
            
          // generating jwt token
            // var token = _token.CreateToken(newUser.Roles.Select(role => role.Name).ToList(), user.Id, 24);
            // newUser.Token = token;

            // Get user by id 
            if (newUser == null) return BadRequest($"Could not register");
            return Ok(newUser);

        }
        
       
        // [HttpPost]
        // public async Task<IActionResult> Refresh(string token, string refreshToken)
        // {
        //     var principal = _token.GetPrincipalFromExpiredToken(token);
        //     var username = principal.Identity.Name; //this is mapped to the Name claim by default
        //
        //     var user = _usersDb.Users.SingleOrDefault(u => u.Username == username);
        //     if (user == null || user.RefreshToken != refreshToken) return BadRequest();
        //
        //     var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
        //     var newRefreshToken = _tokenService.GenerateRefreshToken();
        //
        //     user.RefreshToken = newRefreshToken;
        //     await _usersDb.SaveChangesAsync();
        //
        //     return new ObjectResult(new
        //     {
        //         token = newJwtToken,
        //         refreshToken = newRefreshToken
        //     });
        // }
        
        [HttpPost("refresh")] 
        public async Task<IActionResult> Refresh ([FromBody]TokenRequest request)
        {
            // var refreshToken = new RefreshToken()
            // {
            //     Id = "",
            //     UserId = "Token",
            //     Token = "Token",
            //     JwtId = "id",
            //     IsUsed = false,
            //     IsRevoked = true,
            //     AddedDate = DateTime.Now,
            //     ExpDate = DateTime.Now
            // };
        
           // var token = VerifyGenerateToken(request);
        
            return Ok("Done");
        
        }
        
        // private async Task<IActionResult> VerifyGenerateToken(TokenRequest request)
        // {
        //     var jwtTokenHandler = new JwtSecurityTokenHandler();
        //
        //     _tokenValidationParams.ValidateLifetime = false; // testing
        //
        //     var tokenVer = jwtTokenHandler.ValidateToken(request.Token, _tokenValidationParams, out var validatedToken);
        //
        //     if (validatedToken is JwtSecurityToken jwtSecurityToken)
        //     {
        //         var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
        //             StringComparison.InvariantCultureIgnoreCase);
        //         if (!result) return null;
        //
        //     }
        //     
        //     var utcExpiryDate = long.Parse(tokenVer.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value)
        //
        //     var expDate = UnixTimeStampToDateTime(utcExpiryDate);
        //
        //     if (expDate > DateTime.Now)
        //     {
        //         return BadRequest("Expired Token !");
        //     }
        //     // Can be normal model construct 
        //     var storedToken = _userService.GetStoredTokenAsync(request.RefreshToken);
        //     // Put Some error for Revoked / Used / Expiry date etc 
        //
        //     storedToken.IsUsed = true;
        //     await _userService.UpdateStoredTokenAsync(storedToken.Id);
        //
        //     AppUser user = await _userService.GetUserByIdAsync(storedToken.UserId);
        //     var token = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24);
        //     return Ok(token);
        // }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10),
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private DateTime UnixTimeStampToDateTime(long utcExpiryDate)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(utcExpiryDate).ToUniversalTime();
            return dateTimeVal;
        }
    }
}