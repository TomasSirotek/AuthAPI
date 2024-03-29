using AuthAPI.Domain.BindingModels;
using AuthAPI.Domain.Models;
using AuthAPI.Engines.Cryptography;
using AuthAPI.Identity.BindingModels;
using AuthAPI.Identity.Models;
using AuthAPI.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthAPI.Controllers.User {
    public class UserController : DefaultController {
        private readonly IUserService _userService;
        private readonly IValidator<UserPostModel> _validator;
        private readonly IValidator<UserPutModel> _validatorUpdate;
        private readonly ICryptoEngine _cryptoEngine;

        public UserController(IUserService userService, IValidator<UserPostModel> validator, IValidator<UserPutModel> validatorUpdate, ICryptoEngine cryptoEngine)
        {
            _userService = userService;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
            _cryptoEngine = cryptoEngine;
        }

        #region GET
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            List<AppUser> userList = await _userService.GetAllUsersAsync();
            if (userList.IsNullOrEmpty())
                return BadRequest($"Could not find any users");
            return Ok(userList);
        }
    
    
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsyncById(string id)
       {
           AppUser user = await _userService.GetUserByIdAsync(id);
            if (user == null!) 
                return BadRequest($"Could not find user with Id : {id}");
            return Ok (user);
        }

        #endregion
    
        #region POST
        [HttpPost()]
        [AllowAnonymous]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> CreateAsync([FromBody]UserPostModel request)
        {
            await _validator.ValidateAndThrowAsync(request);
            
            AppUser user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = request.Password,
                CreatedAt = DateTime.Now,
                IsActivated = request.IsActivated
            };
            Address address = new Address()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Street = request.Address.Street,
                Number = request.Address.Number,
                Country = request.Address.Country,
                Zip = request.Address.Zip
                
            };
            user.Address = address;
            AppUser resultUser = await _userService.CreateUserAsync(user,request.Roles, request.Password);
            return Ok(resultUser);
        }
        #endregion
        
        #region PUT
        [HttpPut()]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> UpdateAsync([FromBody]UserPutModel request)
        {
            await _validatorUpdate.ValidateAndThrowAsync(request);
            
            AppUser fetchedUser = await _userService.GetUserByIdAsync(request.Id);
            if(fetchedUser == null!) 
                return BadRequest($"Could not find user with Id : {request.Id}");
            
            AppUser updatedUser = await _userService.UpdateUserAsync(request);
            if(updatedUser == null!) 
                return BadRequest($"Could not update user with Id : {request.Id}");
            return Ok(updatedUser);
        }
        
        [HttpPut("change-password")]
        [Authorize(Roles ="User")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody]PasswordPutModel request)
        {
            //await _validatorUpdate.ValidateAndThrowAsync(request);
            AppUser fetchedUser = await _userService.GetUserByIdAsync(request.UserId);
            if(fetchedUser == null!) 
                return BadRequest($"Could not find user with Id : {request.UserId}");
          
            if(!_cryptoEngine.HashCheck(fetchedUser.PasswordHash, request.CurrentPassword)) 
                return BadRequest($"Current password does not match the original password {request.UserId}");
            
            if (_cryptoEngine.HashCheck(fetchedUser.PasswordHash, request.NewPassword))
                return BadRequest($"New password cannot be same as your original password {request.UserId}");

            var result = await _userService.ChangePasswordAsync(request.UserId,request.NewPassword);
            if(!result) 
                return BadRequest($"Could not update password for user with Id : {request.UserId}");
            // Could use some email notification for user via email 
            return Ok($"Password for user with {request.UserId} has been changed ");
        }

        #endregion

    
        #region DELETE

        [HttpDelete("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            AppUser fetchedUser = await _userService.GetUserByIdAsync(id);
            if(fetchedUser == null!) 
                BadRequest($"Could not find user with {id}");
            var result = await _userService.DeleteAsync(id);
            if(!result) 
                BadRequest($"Could not delete user with {id}");
            return Ok($"User with Id : {id} has been deleted !");
        }
        #endregion
    
    }
}