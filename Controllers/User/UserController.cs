using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Identity.BindingModels;
using ProductAPI.Identity.Models;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers.User {
    public class UserController : DefaultController {
        private readonly IUserService _userService;
        private readonly IValidator<UserPostModel> _validator;
        private readonly IValidator<UserPutModel> _validatorUpdate;

        public UserController(IUserService userService, IValidator<UserPostModel> validator, IValidator<UserPutModel> validatorUpdate)
        {
            _userService = userService;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }

        #region GET
        [HttpGet()]
       // [Authorize(Roles ="Administrator")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            List<AppUser> userList = await _userService.GetAllUsersAsync();
            if (userList.IsNullOrEmpty())
                return BadRequest($"Could not find any users");
            return Ok(userList);
        }
    
    
        [HttpGet("{id}")]
       //  [AllowAuthorizedAttribute(AccessRole.Admin)]
        public async Task<IActionResult> GetAsyncById(string id)
       {
           AppUser user = await _userService.GetUserByIdAsync(id);
            if (user is null) 
                return BadRequest($"Could not find user with Id : {id}");
            return Ok (user);
        }

        #endregion
    
        #region POST
        [HttpPost()]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
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
            AppUser resultUser = await _userService.CreateUserAsync(user,request.Roles, request.Password);
        
            if(resultUser is null) 
                return BadRequest($"Could not create user with Email : {request.Email}");
            return Ok(resultUser);
        }
 
    
        #endregion
        
        #region PUT
        [HttpPut()]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody]UserPutModel request)
        {
            await _validatorUpdate.ValidateAndThrowAsync(request);
            
            AppUser fetchedUser = await _userService.GetUserByIdAsync(request.Id);
            if(fetchedUser == null) 
                return BadRequest($"Could not find user with Id : {request.Id}");
            
            AppUser updatedUser = await _userService.UpdateUserAsync(request);
            if(updatedUser == null) 
                return BadRequest($"Could not update user with Id : {request.Id}");
            return Ok(updatedUser);
        }
        
        #endregion

    
        #region DELETE

        [HttpDelete("{id}")]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            AppUser fetchedUser = await _userService.GetUserByIdAsync(id);
            if(fetchedUser == null) 
                BadRequest($"Could not find user with {id}");
            bool result = await _userService.DeleteAsync(id);
            if(result == null) 
                BadRequest($"Could not delete user with {id}");
            return Ok($"User with Id : {id} has been deleted !");
        }
        #endregion
    
    }
}