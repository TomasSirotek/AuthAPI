using AuthAPI.Identity.BindingModels;
using AuthAPI.Identity.Models;
using AuthAPI.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthAPI.Controllers.User {
    public class RoleController : DefaultController {
        private readonly IRoleService _roleService;
        private readonly IValidator<RolePostModel> _validator;
        private readonly IValidator<RolePutModel> _putValidator;

        public RoleController(IRoleService roleService, IValidator<RolePostModel> validator,
            IValidator<RolePutModel> putValidator)
        {
            _roleService = roleService;
            _validator = validator;
            _putValidator = putValidator;
        }

        #region GET

        [HttpGet()]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<UserRole> roles = await _roleService.GetAsync();
            if (roles.IsNullOrEmpty())
                return BadRequest($"Could not find any category");
            return Ok(roles);
        }

        [HttpGet("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> GetAsyncById(string id)
        {
            UserRole role = await _roleService.GetAsyncById(id);
            if (role == null)
                return BadRequest($"Could not role with Id: {id}");
            return Ok(role);
        }

        [HttpGet("name")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> GetAsyncByName(string name)
        {
            UserRole role = await _roleService.GetAsyncByName(name);
            if (role == null)
                return BadRequest($"Could not find role with name: {name}");
            return Ok(role);
        }

        #endregion

        #region POST

        [HttpPost()]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> CreateAsync([FromBody] RolePostModel request)
        {
            await _validator.ValidateAndThrowAsync(request);
            UserRole modelRole = new UserRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name
            };
            UserRole roleResult = await _roleService.CreateAsync(modelRole);
            if (roleResult == null)
                return BadRequest($"Could not create user with Email : {request.Name}");
            return Ok(roleResult);
        }

        #endregion

        #region PUT

        [HttpPut()]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> UpdateAsync([FromBody] RolePutModel request)
        {
            await _putValidator.ValidateAndThrowAsync(request);

            UserRole requestRole = new UserRole
            {
                Id = request.Id,
                Name = request.Name
            };
            UserRole updatedRole = await _roleService.UpdateAsync(requestRole);
            if (updatedRole == null)
                return BadRequest($"Could not update role with {request.Id}");
            return Ok(updatedRole);
        }

        #endregion

        #region DELETE

        [HttpDelete("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            UserRole fetchedRole = await _roleService.GetAsyncById(id);
            if (fetchedRole == null)
                return BadRequest($"Could not find role with {id}");
            bool result = await _roleService.DeleteAsync(fetchedRole.Id);
            if (!result)
                return
                    BadRequest($"Could not delete role with {id}");
            return Ok($"Role with {id} has been deleted !");
        }

        #endregion
    }
}