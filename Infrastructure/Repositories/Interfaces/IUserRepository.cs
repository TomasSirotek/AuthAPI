using ProductAPI.Identity.Models;

namespace ProductAPI.Infrastructure.Repositories.Interfaces {
    public interface IUserRepository {
        
        Task<List<AppUser>> GetAllUsersAsync();

        Task<AppUser> GetUserByIdAsync(string id);

        Task<AppUser> GetAsyncByEmailAsync(string email);
		
        Task<AppUser> CreateUserAsync(AppUser user);

        Task<AppUser> AddToRoleAsync(AppUser user, UserRole role);
        
        Task<bool> RemoveUserRoleAsync(string roleId);

        Task<bool> ChangePasswordAsync(AppUser user, string newPasswordHash);

        Task<AppUser> UpdateAsync(AppUser user);

        Task<bool> SetActiveAsync(string id, bool result);
        Task<bool> DeleteUser(string id);
    }
}