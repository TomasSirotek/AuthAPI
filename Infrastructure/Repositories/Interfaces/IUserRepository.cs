using ProductAPI.Domain.Models;
using ProductAPI.Identity.Models;

namespace ProductAPI.Infrastructure.Repositories.Interfaces {
    public interface IUserRepository {
        
        Task<List<AppUser>> GetAllUsersAsync();

        Task<AppUser> GetUserByIdAsync(string id);

        Task<AppUser> GetAsyncByEmailAsync(string email);
        
        Task<EmailToken> GetTokenByUserId(string id);
		
        Task<AppUser> CreateUserAsync(AppUser user);

        Task<bool> UpdateToken(RefreshToken refreshToken);

        Task<RefreshToken> FindByTokenAsync(string token);

        Task<AppUser> AddToRoleAsync(AppUser user, UserRole role);
        
        Task<bool> RemoveUserRoleAsync(string roleId);

        Task<bool> ChangePasswordAsync(AppUser user, string newPasswordHash);

        Task<AppUser> UpdateAsync(AppUser user);

        Task<bool> SetActiveAsync(string id);
        
        Task<EmailToken> CreateEmailToken(string userId, EmailToken newToken);
        
        Task<bool> DeleteUser(string id);
    }
}