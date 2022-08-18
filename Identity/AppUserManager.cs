using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProductAPI.Configuration.Token;
using ProductAPI.Engines.Cryptography;
using ProductAPI.Identity.Models;

namespace ProductAPI.Identity; 

public class AppUserManager<TUser> : UserManager<AppUser>, IRoleStore<AppUser> {
    
    // Inject UserStore 

    private readonly AppUserStore _userStore;
    private readonly ICryptoEngine _cryptoEngine;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtToken _token;
    public AppUserManager(AppUserStore userStore,IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<AppUser> passwordHasher, IEnumerable<IUserValidator<AppUser>> userValidators, IEnumerable<IPasswordValidator<AppUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AppUser>> logger, ICryptoEngine cryptoEngine, IJwtToken token, SignInManager<AppUser> signInManager) : base(userStore, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _userStore = userStore;
        _cryptoEngine = cryptoEngine;
        _token = token;
        _signInManager = signInManager;
    }


    public async Task<AppUser> RegisterUserAsync(AppUser user, string password,CancellationToken cancellationToken)
    {
        List<string> roles = new() {"User"};
            
        AppUser newUser = await CreateAsync(user, roles, password,cancellationToken);
        return newUser;
    }
    public async Task<AppUser> CreateAsync(AppUser user, List<string> roles, string password,CancellationToken cancellationToken)
    {
        List<UserRole> appRoles = new();
        List<UserRole> userRoles = appRoles;
        foreach (var role in roles)
        {
            var roleList = new UserRole
            {
                Name = role
            };
            userRoles.Add(roleList);
        }
        user.Roles = userRoles;
        
        var hashedPsw =  _cryptoEngine.Hash(password);
        user.PasswordHash = hashedPsw;
            
        IdentityResult createdUser = await _userStore.CreateAsync(user,cancellationToken);
        
        
        // Create user to sign him in 
        
        // if (createdUser == null)
        //     throw new Exception("Could now create user");
        // foreach (UserRole role in user.Roles)
        // {
        //     UserRole fetchedRole = await _roleRepository.GetRoleAsyncByName(role.Name);
        //     if (fetchedRole == null) 
        //         throw new Exception("Could now create roles for user");
        //     await _userRepository.AddToRoleAsync(createdUser, fetchedRole);
        // }
        // if (user is {IsActivated: false})
        // {
        //     var confirmEmailToken = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24);;
        //     //  var link = $"https://localhost:5000/Authenticate/confirm?userId={user.Id}&token={confirmEmailToken}";
        //     //  _emailService.SendEmail(user.Email,user.UserName,link,"Confirm email");
        // }
       //  AppUser fetchedNewUser = await _userStore.GetUserByIdAsync(user.Id);
       return null;
    }


    public async Task<IdentityResult> CreateAsync(AppUser role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> UpdateAsync(AppUser role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> DeleteAsync(AppUser role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetRoleIdAsync(AppUser role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetRoleNameAsync(AppUser role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SetRoleNameAsync(AppUser role, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetNormalizedRoleNameAsync(AppUser role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SetNormalizedRoleNameAsync(AppUser role, string normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<AppUser> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<AppUser> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}