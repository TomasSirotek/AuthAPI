using AuthAPI.Domain.Models;
using AuthAPI.Identity.Models;

namespace AuthAPI.Infrastructure.Repositories.Interfaces {
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

        Task<Address> AddAddressToUser(string userId, Address address);

        Task<bool> ChangePasswordAsync(string userId, string newPasswordHash);

        Task<AppUser> UpdateAsync(AppUser user);

        Task<bool> SetActiveAsync(string id);
        
        Task<EmailToken> CreateEmailToken(string userId, EmailToken newToken);
        
        Task<bool> DeleteUser(string id);
    }
}