using Microsoft.AspNetCore.Identity;
using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;

namespace ProductAPI.Identity; 

public class AppUserStore : IUserStore<AppUser> {

    private readonly IUserRepository _userRepository;

    public AppUserStore (IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    // Direct to Repository // inject repository
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
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
    
    // return identity Result 
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

    public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}