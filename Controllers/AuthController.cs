using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Configuration.Token;
using ProductAPI.Domain.Models;
using ProductAPI.Engines;
using ProductAPI.Engines.Cryptography;
using ProductAPI.Helpers;
using ProductAPI.Identity.BindingModels.Authentication;
using ProductAPI.Identity.Models;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers {
    public class AuthController : DefaultController {
        private readonly IJwtToken _token;
        private readonly ICryptoEngine _cryptoEngine;
        private readonly IUserService _userService;
        private readonly IVerifyJwtToken _validJwtHelper;
        
        public AuthController(IJwtToken token, ICryptoEngine cryptoEngine, IUserService userService, IVerifyJwtToken validJwtHelper)
        {
            _token = token;
            _cryptoEngine = cryptoEngine;
            _userService = userService;
            _validJwtHelper = validJwtHelper;
        }

        [HttpPost()]
        public async Task<ActionResult> Authenticate([FromBody] AuthPostModel request)
        {
            AppUser user = await _userService.GetAsyncByEmailAsync(request.Email);

            if (user == null || !_cryptoEngine.HashCheck(user.PasswordHash, request.Password))
                return BadRequest("Email or password is incorrect");

            var token = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id);
            user.Token = token;
            
            RefreshToken refreshToken = await _token.GenerateRefreshToken(user.Id);
            user.RefreshToken = refreshToken.Token;

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterPostModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser fetchedUser = await _userService.GetAsyncByEmailAsync(model.Email);
                if (fetchedUser != null) return BadRequest($"User with email {model.Email} already exists");
            }

            AppUser user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsActivated = false,
                CreatedAt = DateTime.Now
            };

            AppUser createdUser = await _userService.RegisterUserAsync(user, model.Password);
            if (createdUser == null) return BadRequest($"Could not register user");

            return Ok(createdUser);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            var result = await _validJwtHelper.VerifyAndGenerateToken(tokenRequest);
            return Ok(result);
        }
        
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userId, token);
            if (result) 
                return Ok("Email confirmed");

            return BadRequest($"Could not confirm account for user with id {userId}");
        }
    }
}