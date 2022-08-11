using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Domain.BindingModels;
using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Repositories;
using ProductAPI.Services;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers {
    public class ProductController : DefaultController {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #region GET
        [HttpGet()]
        // [AllowAuthorized(AccessRoles.Admin)]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAllAsync ()
        {
            List<Product> productList = await _productService.GetAsync();
            if (productList.IsNullOrEmpty())
                return BadRequest($"Could not find any products");
            return Ok(productList);
        }
    
    
        [HttpGet("{id}")]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
        public async Task<IActionResult> GetAsyncById(string id)
        {
            Product product = await _productService.GetByIdAsync(id);
            if (product != null) 
                return Ok (product);
            return BadRequest($"Could not find product with Id : {id}");
        }

        #endregion
    
        #region POST
        [HttpPost()]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
        public async Task<IActionResult> CreateAsync([FromBody]PostProductModel request)
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title,
                Description = request.Description,
                Image = request.Image,
                IsActive = request.IsActive,
                UnitPrice = request.UnitPrice,
                UnitsInStock = request.UnitsInStock
            };
            Product resultProduct = await _productService.CreateAsync(product,request.Category);
            
            Product fetchedDbProduct = await _productService.GetByIdAsync(resultProduct.Id);
            if(fetchedDbProduct == null) 
                return BadRequest($"Could not create product");
            return Ok(fetchedDbProduct);
        }
 
    
        #endregion
        
        #region PUT
        [HttpPut()]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody]PutProductModel request)
        {
            if (request.Id == null) return BadRequest($"Could not find product with Id {request.Id}");
            Product resultProduct = await _productService.UpdateAsync(request);
            Product createdProduct = await _productService.GetByIdAsync(resultProduct.Id);
            return Ok(createdProduct);
                
        }
        
        #endregion

    
        #region DELETE

        [HttpDelete("{id}")]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            Product fetchedProduct = await _productService.GetByIdAsync(id);
            if(fetchedProduct == null) BadRequest($"Could not find product with {id}");
            bool result = await _productService.DeleteAsync(fetchedProduct.Id); 
            if(result == null) BadRequest($"Could not delete product with {id}");
            return Ok($"Product with Id : {id} has been deleted !");
        }
        #endregion
    
    }
}