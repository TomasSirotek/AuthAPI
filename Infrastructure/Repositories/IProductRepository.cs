using ProductAPI.Domain.Models;

namespace ProductAPI.Infrastructure.Repositories; 

public interface IProductRepository {

    Task<List<Product>> GetAsync();
    
    Task<Product> GetByIdAsync(string id);

    Task<Product> CreateAsync(Product character);
        
    Task<Product> UpdateAsync(Product character);
        
    Task<bool> DeleteAsync(string id);
}