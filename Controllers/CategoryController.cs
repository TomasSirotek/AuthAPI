using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Domain.BindingModels;
using ProductAPI.Domain.BindingModels.Category;
using ProductAPI.Domain.Models;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers; 

public class CategoryController : DefaultController {
    private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // TODO: Better error handling 

        #region GET
        [HttpGet()]
        // [AllowAuthorized(AccessRoles.Admin)]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAllAsync ()
        {
            List<Category> categoryList = await _categoryService.GetAsync();
            if (categoryList.IsNullOrEmpty())
                return BadRequest($"Could not find any category");
            return Ok(categoryList);
        }
    
    
        [HttpGet("{id}")]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
        public async Task<IActionResult> GetAsyncById(string id)
        {
            Category category = await _categoryService.GetByIdAsync(id);
            if (category != null) 
                return Ok (category);
            return BadRequest($"Could not find category with Id : {id}");
        }

        #endregion
    
        #region POST
        [HttpPost()]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
        public async Task<IActionResult> CreateAsync([FromBody]PostCategoryModel request)
        {
            Category category = new Category()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
            };
            Category resultCategory = await _categoryService.CreateAsync(category);
            
            if(resultCategory == null) 
                return BadRequest($"Could not create category");
            return Ok(resultCategory);
        }
 
    
        #endregion
        #region PUT
        [HttpPut()]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody]PutCategoryModel request)
        {
            Category fetchedCategory = await _categoryService.GetByIdAsync(request.Id);
            if (fetchedCategory != null)
            {
                Category updatedCategory = new Category()
                {
                    Id = request.Id,
                    Name = request.Name,
                };
                Category resultCategory = await _categoryService.UpdateAsync(updatedCategory);
               
                if(resultCategory == null) 
                    return BadRequest($"Could not create category");
                return Ok(resultCategory);
            }
            return BadRequest($"Could not create category");
        }
        
        #endregion

    
        #region DELETE

        [HttpDelete("{id}")]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            Category fetchedCategory = await _categoryService.GetByIdAsync(id);
            if(fetchedCategory == null) BadRequest($"Could not find category with {id}");
            bool result = await _categoryService.DeleteAsync(fetchedCategory.Id); 
            if(result == null) BadRequest($"Could not delete category with {id}");
            return Ok($"Category with Id : {id} has been deleted !");
        }
        #endregion
    
    }
    