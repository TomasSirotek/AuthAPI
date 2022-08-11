using ProductAPI.Configuration.Token;
using ProductAPI.Engines.Cryptography;
using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services {
    public class UserService : IUserService{
        
        private readonly IUserRepository _userRepository;
      //  private readonly IRoleRepository _roleRepository;
      //  private readonly IEmailService _emailService;
        private readonly ICryptoEngine _cryptoEngine;
        private readonly IJwtToken _token;
        
        public UserService (IUserRepository userRepository,ICryptoEngine cryptoEngine,IJwtToken token)
        {
            _userRepository = userRepository;
           // _emailService = emailService;
            _cryptoEngine = cryptoEngine;
            // _roleRepository = roleRepository;
           _token = token;

        }
        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<AppUser> GetAsyncByEmailAsync(string email)
        {
            return await _userRepository.GetAsyncByEmailAsync(email);
        }

        public async Task<AppUser> RegisterUserAsync(AppUser user, string password)
        {
            List<string> roles = new() {"User"};
            
            AppUser newUser = await CreateUserAsync(user, roles, password);
            return newUser;
        }

        public async Task<AppUser> CreateUserAsync(AppUser user, List<string> roles, string password)
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
            
            AppUser createdUser = await _userRepository.CreateUserAsync(user);
            if (createdUser == null)
                throw new Exception("Could now create user");
            foreach (UserRole role in user.Roles)
            {
              //  UserRole fetchedRole = await _roleRepository.GetAsyncByName(role.Name);
                // if (fetchedRole == null) 
                //     throw new Exception("Could now create roles for user");
                // await _userRepository.AddToRoleAsync(createdUser, fetchedRole);
            }
            if (user is {IsActivated: false})
            {
                var confirmEmailToken = _token.CreateToken(user.Roles.Select(role => role.Name).ToList(), user.Id, 24);;
              //  var link = $"https://localhost:5000/Authenticate/confirm?userId={user.Id}&token={confirmEmailToken}";
              //  _emailService.SendEmail(user.Email,user.UserName,link,"Confirm email");
            }
            AppUser fetchedNewUser = await _userRepository.GetUserByIdAsync(createdUser.Id);
            return fetchedNewUser;
        }

        public async Task<AppUser> UpdateUserAsync(AppUser user)
        {
            AppUser updatedUser = await _userRepository.UpdateAsync(user);
            if (updatedUser == null) throw new Exception("Could not update user ");
           
            foreach (UserRole category in updatedUser.Roles)
            {
               // bool removeCategory = await _userRepository.RemoveUserRoleAsync(category.Id);
              //  if (!removeCategory) throw new Exception("Could not delete category ");
            }
         
            foreach (UserRole role in user.Roles)
            {
              //  UserRole fetchedRole = await _roleRepository.GetRoleAsyncByName(role.Name);
              //  if(fetchedRole == null)  throw new Exception($"Could not find role with name {role.Name}");
                // await _userRepository.AddToRoleAsync(updatedUser, fetchedRole);
            }
                
            return updatedUser;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _userRepository.DeleteUser(id);
        }

        public Task<bool> ChangePasswordAsync(AppUser user, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}