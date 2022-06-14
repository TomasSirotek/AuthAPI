using Data_Access.Models;

namespace Data_Access.Repositories; 

public interface ICharacterRepository {
    Task SaveAsync();
    
    Task<List<Product>> GetAsync();
    
    Task<Product> GetByIdAsync(string id);

    Task<Product> CreateAsync(Product character);
        
    Task<Product> UpdateAsync(Product character);
        
    Task<bool> DeleteAsync(string id);
}