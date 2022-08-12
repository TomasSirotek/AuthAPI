using ProductAPI.Identity.Models;

namespace ProductAPI.Infrastructure.Repositories.Interfaces {
    public interface IRoleRepository {
        Task<List<UserRole>> GetRoleAsync();
        
        Task<UserRole> GetRoleAsyncByName(string name);
        
        Task<UserRole> GetRoleByIdAsync(string id);

        Task<bool> CreateRoleAsync(UserRole role);

        Task<UserRole> UpdateRoleAsync(UserRole role);

        Task<bool> DeleteAsync(string id);
    }
}