using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProductAPI.Configuration.Token;
using ProductAPI.Domain.Models;
using ProductAPI.Engines.Cryptography;
using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;

namespace ProductAPI.Identity; 

public class AppUserManager<TUser> : UserManager<AppUser>{
    
    // Inject UserStore 

    private readonly AppUserStore _userStore;
    private readonly IRoleRepository _roleRepository;
    private readonly ICryptoEngine _cryptoEngine;
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtToken _token;
    public AppUserManager(AppUserStore userStore,IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<AppUser> passwordHasher, IEnumerable<IUserValidator<AppUser>> userValidators, IEnumerable<IPasswordValidator<AppUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AppUser>> logger, ICryptoEngine cryptoEngine, IJwtToken token, SignInManager<AppUser> signInManager, IRoleRepository roleRepository, IUserRepository userRepository) : base(userStore, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _userStore = userStore;
        _cryptoEngine = cryptoEngine;
        _token = token;
        _signInManager = signInManager;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
    }
    public async Task<List<AppUser>> GetAllUsersAsync()
    {
        return await _userStore.GetAllAsync();
    }
    
    public async Task<AppUser> GetAsyncByEmailAsync(string email)
    {
        return await _userStore.GetAsyncByEmailAsync(email);
    }
    public async Task<IdentityResult> RegisterUserAsync(AppUser user, string password,CancellationToken cancellationToken)
    {
        List<string> roles = new() {"User"};
            
        AppUser newUser = await CreateAsync(user, roles, password,cancellationToken);
        if(newUser != null) return IdentityResult.Success;
       throw new Exception("Could now create user");
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
        
        if (createdUser.Succeeded)
        {
            AppUser fetchedNewUser = await _userStore.FindByIdAsync(user.Id,cancellationToken);
            
            foreach (UserRole role in user.Roles)
            {
                UserRole fetchedRole = await _roleRepository.GetRoleAsyncByName(role.Name);
                if (fetchedRole == null) 
                    throw new Exception("Could now create roles for user");
                await _userRepository.AddToRoleAsync(fetchedNewUser, fetchedRole);
            }
            return fetchedNewUser;
        }
     
        throw new Exception("Could now create roles for user");
        
        // if (user is {IsActivated: false})
        // {
        //     var confirmEmailToken = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24);;
        //     //  var link = $"https://localhost:5000/Authenticate/confirm?userId={user.Id}&token={confirmEmailToken}";
        //     //  _emailService.SendEmail(user.Email,user.UserName,link,"Confirm email");
        // }
       //  AppUser fetchedNewUser = await _userStore.GetUserByIdAsync(user.Id);
    }
    

    public async Task<RefreshToken>  FindTokenAsync(string token)
    {
        return await _userStore.FindByTokenAsync(token);
    }
   
    
    public async Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _userStore.DeleteAsync(id);
    }
    
    

  
}