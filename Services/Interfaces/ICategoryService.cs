using ProductAPI.Domain.Models;

namespace ProductAPI.Services.Interfaces {
    public interface ICategoryService {
        Task<List<Category>> GetAsync();
    
        Task<Category> GetByIdAsync(string id);

        Task<Category> CreateAsync(Category product);
        
        Task<Category> UpdateAsync(Category product);
        
        Task<bool> DeleteAsync(string id);
    }
}