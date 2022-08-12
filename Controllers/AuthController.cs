using Microsoft.AspNetCore.Mvc;
using ProductAPI.Configuration.Token;
using ProductAPI.Engines.Cryptography;
using ProductAPI.Identity.BindingModels.Authentication;
using ProductAPI.Identity.Models;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers {
    
    public class AuthController : DefaultController{
        
        private readonly IJwtToken _token;
        private readonly IUserService _userService;
        private readonly ICryptoEngine _cryptoEngine;
        
        public AuthController (IJwtToken token,IUserService userService,ICryptoEngine cryptoEngine)
        {
            _token = token;
            _userService = userService;
            _cryptoEngine = cryptoEngine;
        }

        [HttpPost ()] 
        public async Task<ActionResult> Authenticate ([FromBody]AuthPostModel request)
        {
            AppUser user = await _userService.GetAsyncByEmailAsync(request.Email);
            if (user == null)
                return BadRequest($"User email: {request.Email} is not correct!");

            if (!_cryptoEngine.HashCheck(user.PasswordHash, request.Password))
                return BadRequest("Password incorrect !");

            var token = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24);
            user.Token = token;
          
            return Ok(user);
        }
	
        [HttpPost ("register")] 
        public async Task<IActionResult> Register ([FromBody]RegisterPostModel model)
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
            AppUser newUser = await _userService.RegisterUserAsync(user, model.Password);
          
            var token = _token.CreateToken(newUser.Roles.Select(role => role.Name).ToList(), user.Id, 24);
            newUser.Token = token;
            if (newUser == null) return BadRequest($"Could not register");
            return Ok(newUser);

        }
    }
}