using ProductAPI.Identity.Models;

namespace ProductAPI.Services.Interfaces {
    public interface IUserService {
        
        Task<List<AppUser>> GetAllUsersAsync();
        
        Task<AppUser> GetUserByIdAsync(string id);

        Task<AppUser> GetAsyncByEmailAsync(string email);

        Task<AppUser> RegisterUserAsync(AppUser user, string password);
        
        Task<AppUser> CreateUserAsync(AppUser user,List<string> roles,string password);
        
        Task<AppUser> UpdateUserAsync(AppUser user);
        
        Task<bool> DeleteAsync(string id);

        Task<bool> ChangePasswordAsync(AppUser user,string newPassword);
    }
}