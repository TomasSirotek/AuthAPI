using AuthAPI.Identity.Models;

namespace AuthAPI.Infrastructure.Repositories.Interfaces {
    public interface IRoleRepository {
        Task<List<UserRole>> GetRoleAsync();
        
        Task<UserRole> GetRoleAsyncByName(string name);
        
        Task<UserRole> GetRoleByIdAsync(string id);

        Task<bool> CreateRoleAsync(UserRole role);

        Task<bool> UpdateRoleAsync(UserRole role);

        Task<bool> DeleteAsync(string id);
    }
}