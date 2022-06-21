using ProductAPI.Domain.Models;

namespace ProductAPI.Infrastructure.Repositories.Interfaces {
    public interface ICategoryRepository {
        Task<List<Category>> GetAsync();
    
        Task<Category> GetByIdAsync(string id);

        Task<Category> CreateAsync(Category category);
        
        Task<Category> UpdateAsync(Category category);
        
        Task<bool> DeleteAsync(string id);
    }
}