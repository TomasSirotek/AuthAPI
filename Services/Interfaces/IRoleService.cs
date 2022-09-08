using AuthAPI.Identity.Models;

namespace AuthAPI.Services.Interfaces; 

public interface IRoleService {
    
    Task<List<UserRole>> GetAsync();
        
    Task<UserRole> GetAsyncById(string id);

    Task<UserRole> GetAsyncByName(string name);

    Task<UserRole> CreateAsync(UserRole role);

    Task<UserRole> UpdateAsync(UserRole role);
        
    Task<bool> DeleteAsync(string id);
}