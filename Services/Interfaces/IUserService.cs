using AuthAPI.Domain.Models;
using AuthAPI.Identity.BindingModels;
using AuthAPI.Identity.Models;

namespace AuthAPI.Services.Interfaces {
    public interface IUserService {
        
        Task<List<AppUser>> GetAllUsersAsync();
        
        Task<AppUser> GetUserByIdAsync(string id);

        Task<AppUser> GetAsyncByEmailAsync(string email);

        Task<RefreshToken> FindTokenAsync(string token);

        Task<AppUser> RegisterUserAsync(AppUser user, string password);
        
        Task<AppUser> CreateUserAsync(AppUser user,List<string> roles,string password);
        
        
        Task<AppUser> UpdateUserAsync(UserPutModel model);
        
        Task<bool> DeleteAsync(string id);

        Task<bool> ConfirmEmailAsync(string userId, string token);

        Task<bool> ChangePasswordAsync(string userId,string newPassword);
    }
}