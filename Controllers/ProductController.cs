using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Domain.BindingModels;
using ProductAPI.Domain.Models;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Controllers; 

public class ProductController : DefaultController {
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    #region GET
    [HttpGet()]
    // [AllowAuthorized(AccessRoles.Admin)]
    //[AllowAnonymous]
    public async Task<IActionResult> GetAllAsync ()
    {
        List<Product> productList = await _productRepository.GetAsync();
        if (productList.IsNullOrEmpty())
            return BadRequest($"Could not find any products");
        return Ok(productList);
    }
    
    
    [HttpGet("{id}")]
    //[AllowAuthorizedAttribute(AccessRoles.Admin)]
    public async Task<IActionResult> GetAsyncById(string id)
    {
        Product product = await _productRepository.GetByIdAsync(id);
        if (product != null) 
            return Ok (product);
        return BadRequest($"Could not find product with Id : {id}");
        return null;
    }

    #endregion
    
    #region POST
    [HttpPost()]
    //[AllowAuthorizedAttribute(AccessRoles.Admin)]
    public async Task<IActionResult> CreateAsync([FromBody]PostProductModel request)
    {
        // move to services 
        Product product = new Product()
       {
           Id = Guid.NewGuid().ToString(),
           Title = request.Title,
           Price = request.Price,
           Description = request.Description,
           Image = request.Image,
           IsAvailable = request.IsAvailable,
           AgeLimit = request.AgeLimit
       };
        Product resultProduct = await _productRepository.CreateAsync(product);
        
        // TODO: Needs to have categories before fetching by Id (for N:N)
        //   Product fetchedDbProduct = await _productRepository.GetByIdAsync(resultProduct.Id);
        if(resultProduct == null) 
            return BadRequest($"Could not create product");
        return Ok(resultProduct);
       return null;
    }
 
    
    #endregion
    
    #region PUT
    [HttpPut()]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> UpdateAsync([FromBody]PostProductModel request)
    {
            // implement update 
        return null;
    }
    
    #endregion

    
    #region DELETE

    [HttpDelete("{id}")]
    //[Authorize(Roles ="Admin")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        Product fetchedProduct = await _productRepository.GetByIdAsync(id);
        if(fetchedProduct == null) BadRequest($"Could not find product with {id}");
        bool result = await _productRepository.DeleteAsync(fetchedProduct.Id); 
        if(result == null) BadRequest($"Could not delete product with {id}");
        return Ok($"Product with Id : {id} has been deleted !");
    }
    #endregion
    
}