using ProductAPI.Domain.BindingModels;
using ProductAPI.Domain.Models;

namespace ProductAPI.Services.Interfaces {
    public interface IProductService {
    
        Task<List<Product>> GetAsync();
    
        Task<Product> GetByIdAsync(string id);

        Task<Product> CreateAsync(Product product,List<string> category);
        
        Task<Product> UpdateAsync(PutProductModel productModel);
        
        Task<bool> DeleteAsync(string id);
    }
}