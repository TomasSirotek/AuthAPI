using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Configuration.Token;
using ProductAPI.Domain.BindingModels;
using ProductAPI.Domain.Models;
using ProductAPI.Engines.Cryptography;
using ProductAPI.Identity.BindingModels.Authentication;
using ProductAPI.Identity.Models;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers {
    public class AuthController : DefaultController {
        private readonly IJwtToken _token;
        private readonly ICryptoEngine _cryptoEngine;
        private readonly IUserService _userService;
        private readonly IVerifyJwtToken _validJwtHelper;
        private readonly IValidator<RegisterPostModel> _validator;
        
        public AuthController(IJwtToken token, ICryptoEngine cryptoEngine, IUserService userService, IVerifyJwtToken validJwtHelper, IValidator<RegisterPostModel> validator)
        {
            _token = token;
            _cryptoEngine = cryptoEngine;
            _userService = userService;
            _validJwtHelper = validJwtHelper;
            _validator = validator;
        }

        [HttpPost()]
        public async Task<ActionResult> Authenticate([FromBody] AuthPostModel request)
        {
            AppUser user = await _userService.GetAsyncByEmailAsync(request.Email);
            // return if user does not exist Bad request already exists 
            
            if (!_cryptoEngine.HashCheck(user.PasswordHash, request.Password))
                return BadRequest("Email or password is incorrect");

            var token = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id);
            user.Token = token;
            
            RefreshToken refreshToken = await _token.GenerateRefreshToken(user.Id);
            user.RefreshToken = refreshToken.Token;

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterPostModel request)
        {
            await _validator.ValidateAsync(request);
            
            AppUser fetchedUser = await _userService.GetAsyncByEmailAsync(request.Email);
            if (fetchedUser is not null)
                    return BadRequest($"User with email {request.Email} already exists");

            AppUser user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                IsActivated = false,
                CreatedAt = DateTime.Now
            };

            AppUser createdUser = await _userService.RegisterUserAsync(user, request.Password);
            return Ok(createdUser);
        }

        
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (string.IsNullOrWhiteSpace(tokenRequest.Token) || string.IsNullOrWhiteSpace(tokenRequest.RefreshToken))
                return NotFound();
            var result = await _validJwtHelper.VerifyAndGenerateToken(tokenRequest);
            return Ok(result);
        }
        
        [HttpPost("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _userService.ConfirmEmailAsync(userId, token);
            if (!result) 
                return BadRequest($"Could not confirm account for user with id {userId}");
            return Ok("Email confirmed");
        }
    }
}