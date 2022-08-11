using ProductAPI.Domain.Models;

namespace ProductAPI.Infrastructure.Repositories.Interfaces {
    public interface IProductRepository {

        Task<List<Product>> GetAsync();
    
        Task<Product> GetByIdAsync(string id);

        Task<Product> CreateAsync(Product product);

        Task<Product> AddCategoryAsync(Product product, Category category);
        
        Task<Product> UpdateAsync(Product product);

        Task<bool> RemoveCategoryAsync(string categoryId);
        
        Task<bool> DeleteAsync(string id);
    }
}