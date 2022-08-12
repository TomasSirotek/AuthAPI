using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Identity.BindingModels;
using ProductAPI.Identity.Models;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers.User {
    public class RoleController : DefaultController{
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
    
        #region GET
        [HttpGet()]
        // [AllowAuthorized(AccessRoles.Admin)]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAllAsync ()
        {
            List<UserRole> roles = await _roleService.GetAsync();
            if (roles.IsNullOrEmpty())
                return BadRequest($"Could not find any category");
            return Ok(roles);
        }
        [HttpGet("{id}")]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
        // [AllowAnonymous]
        public async Task<IActionResult> GetAsyncById (string id)
        {
            UserRole role =  await _roleService.GetAsyncById(id);
            if(role == null) 
                return BadRequest($"Could not role with Id: {id}");
            return Ok(role);
        }
    
        [HttpGet("name")]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
        public async Task<IActionResult> GetAsyncByName(string name)
        {
            UserRole role = await _roleService.GetAsyncByName(name);
            if (role == null) 
                return BadRequest($"Could not find role with name: {name}");
            return Ok (role);
        }
    
        #endregion
    
        #region POST
        [HttpPost()]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
        public async Task<IActionResult> CreateAsync([FromBody]RolePostModel model)
        {
            // move to services 
            UserRole modelRole = new UserRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name
            };
            UserRole roleResult = await _roleService.CreateAsync(modelRole);
            if(roleResult == null) 
                return BadRequest($"Could not create user with Email : {model.Name}");
            return Ok(roleResult);
        }
        #endregion
    
        #region PUT
        [HttpPut()]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
        public async Task<IActionResult> UpdateAsync([FromBody]RolePutModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Id) || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest($"Could not create user with Id : {request.Id}");
        
            UserRole requestRole = new UserRole
            {
                Id = request.Id,
                Name = request.Name
            };
            UserRole updatedRole = await _roleService.UpdateAsync(requestRole);
            if (updatedRole == null) BadRequest($"Could not update role with {request.Id}");
            return Ok(updatedRole);
        }
    
    
        #endregion
    
        #region DELETE
        [HttpDelete]
        // [AllowAuthorizedAttribute(AccessRoles.Admin)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            UserRole fetchedRole = await _roleService.GetAsyncById(id);
            if(fetchedRole == null) BadRequest($"Could not find role with {id}");
            bool result = await _roleService.DeleteAsync(fetchedRole.Id); 
            if(result == null) BadRequest($"Could not delete role with {id}");
            return Ok($"Role with {id} has been deleted !");
        }
    
        #endregion

    
    }
}