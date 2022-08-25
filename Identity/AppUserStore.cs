using Microsoft.AspNetCore.Identity;
using ProductAPI.Domain.Models;
using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;

namespace ProductAPI.Identity; 

public class AppUserStore : IUserStore<AppUser> {

    private readonly IUserRepository _userRepository;

    public AppUserStore (IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
    #region GET
    public async Task<List<AppUser>> GetAllAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }
    
    public async Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }
    
    public async Task<AppUser> GetAsyncByEmailAsync(string email)
    {
        return await _userRepository.GetAsyncByEmailAsync(email);
    }
    
    
    public async Task<RefreshToken> FindByTokenAsync(string token)
    {
        return await _userRepository.FindByTokenAsync(token);
    }
    #endregion

    #region POST
    public async Task<IdentityResult> CreateAsync(AppUser user,CancellationToken cancellationToken)
    {
        AppUser createdUser = await _userRepository.CreateUserAsync(user);
        
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (createdUser != null)
        {
            return IdentityResult.Success;
        }
        else
        {
            return IdentityResult.Failed();
        }
        
    }
    #endregion

    #region PUT
    public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    #endregion

    #region DELETE

    public async Task<bool> DeleteAsync(string id)
    {
        bool result =  await _userRepository.DeleteUser(id);
        if (result) return true;
        return false;
    }
    #endregion
   
   
    
    public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    
    
    
    
    // Not needed for now 
    public Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    
    public Task<AppUser> GetUserByRefreshToken(string token)
    {
        throw new NotImplementedException();
    }
    public async Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    
}