using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Configuration.Token;
using ProductAPI.Domain.Models;
using ProductAPI.Engines.Cryptography;
using ProductAPI.Identity;
using ProductAPI.Identity.BindingModels.Authentication;
using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;

namespace ProductAPI.Controllers {
    
    public class AuthController : DefaultController{
        
        private readonly IJwtToken _token;
        private readonly ICryptoEngine _cryptoEngine;
        private readonly IUserRepository _userRepository;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly AppUserManager<AppUser> _userManager;
        
        
        public AuthController (IJwtToken token,ICryptoEngine cryptoEngine,TokenValidationParameters tokenValidationParams, AppUserManager<AppUser> userManager, IUserRepository userRepository)
        {
            _token = token;
            _cryptoEngine = cryptoEngine;
            _tokenValidationParams = tokenValidationParams;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpPost ()] 
        public async Task<ActionResult> Authenticate ([FromBody]AuthPostModel request)
        {
            AppUser user = await _userManager.GetAsyncByEmailAsync(request.Email);
            
            if (user == null || !_cryptoEngine.HashCheck(user.PasswordHash, request.Password))
                return BadRequest("Email or password is incorrect");
            
            var token = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24); // 24
            user.Token = token;
            
            // Create refresh token
            RefreshToken refreshToken = await _token.GenerateRefreshToken(user.Id);
           
            user.RefreshToken = refreshToken.Token;
            return Ok(user);
        }
        
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult>  RefreshToken([FromBody] TokenRequest tokenRequest)
        {

            var result = await VerifyAndGenerateToken(tokenRequest);
            return Ok(result);
        }

        private async Task<string> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            _tokenValidationParams.ValidateLifetime = false;
            var tokenVerification =
                jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,StringComparison.InvariantCultureIgnoreCase);
                if (!result)
                    return null;
            }
            
            var utcExpiryDate = long.Parse(tokenVerification.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
            
            if (expiryDate > DateTime.Now)
            {
               Console.WriteLine("nope");
            }
            RefreshToken storedToken = await _userManager.FindTokenAsync(tokenRequest.RefreshToken);
            AppUser user= await _userManager.FindByIdAsync(storedToken.UserId);
            var token = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24); // 24
            return token;

        }

        // helper methods
        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }
        
        [HttpPost ("register")] 
        public async Task<IActionResult> Register ([FromBody]RegisterPostModel model,CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                AppUser fetchedUser = await _userManager.GetAsyncByEmailAsync(model.Email);
                if (fetchedUser != null) return BadRequest($"User with email {model.Email} already exists");
            }
		
            AppUser user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsActivated = true,
                CreatedAt = DateTime.Now
            };
            
            IdentityResult identityResult = await _userManager.RegisterUserAsync(user, model.Password,cancellationToken);
            if (!identityResult.Succeeded) return BadRequest($"Could not register user");
            
            AppUser createdUser = await _userManager.FindByIdAsync(user.Id);
  
            var token = _token.CreateToken(createdUser.Roles.Select(role => role.Name).ToList(), user.Id, 24);
            createdUser.Token = token;
            
            return Ok(createdUser);

        }
        
       
    }
}