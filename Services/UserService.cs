using System.Security.Cryptography;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Identity;
using ProductAPI.Configuration.Token;
using ProductAPI.Domain.Enum;
using ProductAPI.Domain.Models;
using ProductAPI.Engines.Cryptography;
using ProductAPI.ExternalServices;
using ProductAPI.Identity.BindingModels;
using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services {
    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICryptoEngine _cryptoEngine;
        private readonly IJwtToken _token;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, ICryptoEngine cryptoEngine, IJwtToken token,
            IRoleRepository roleRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _cryptoEngine = cryptoEngine;
            _token = token;
            _roleRepository = roleRepository;
            _emailService = emailService;
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

        public async Task<RefreshToken> FindTokenAsync(string token)
        {
            return await _userRepository.FindByTokenAsync(token);
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

            var hashedPsw = _cryptoEngine.Hash(password);
            user.PasswordHash = hashedPsw;

            AppUser createdUser = await _userRepository.CreateUserAsync(user);
            if (createdUser == null)
                throw new Exception("Could not create user");

            foreach (UserRole role in user.Roles)
            {
                UserRole fetchedRole = await _roleRepository.GetRoleAsyncByName(role.Name);
                if (fetchedRole == null)
                    throw new Exception("Could now create roles for user");
                await _userRepository.AddToRoleAsync(createdUser, fetchedRole);
            }

            if (user is {IsActivated: false})
            {
                var token = Guid.NewGuid().ToString();

                EmailToken verifyToken = await _userRepository.GetTokenByUserId(createdUser.Id);
                if (verifyToken?.Token == token)
                    throw new Exception("Could generate token because it exists");

                EmailToken newEmailToken = new EmailToken()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = createdUser.Id,
                    Token = token,
                    CreatedAt = DateTime.Now,
                    IsUsed = false,
                };

                EmailToken confirmEmailToken = await _userRepository.CreateEmailToken(createdUser.Id, newEmailToken);

                var link =
                    $"https://localhost:5000/Auth/confirm-email?userId={createdUser.Id}&token={confirmEmailToken.Token}";

                var email = new EmailModel()
                {
                    EmailTo = user.Email,
                    Name = user.FirstName,
                    Body = link,
                    Subject = Status.CONFIRM_EMAIL.ToString()
                };
                _emailService.SendEmail(email);
            }

            AppUser fetchedNewUser = await _userRepository.GetUserByIdAsync(createdUser.Id);
            return fetchedNewUser;
        }


        public async Task<AppUser> UpdateUserAsync(UserPutModel model)
        {
            AppUser requestUser = new AppUser()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsActivated = model.IsActivated,
                UpdatedAt = DateTime.Now
            };
            AppUser updatedUser = await _userRepository.UpdateAsync(requestUser);

            if (updatedUser == null) throw new Exception("Could not update user ");
            AppUser fetchedUser = await _userRepository.GetUserByIdAsync(model.Id);

            foreach (UserRole oldRole in fetchedUser.Roles)
            {
                bool removeCategory = await _userRepository.RemoveUserRoleAsync(oldRole.Id);
                if (!removeCategory) throw new Exception("Could not delete role ");
            }

            foreach (var name in model.Roles)
            {
                UserRole fetchedRole = await _roleRepository.GetRoleAsyncByName(name);
                if (fetchedRole == null) throw new Exception($"Could not find role with name {name}");
                await _userRepository.AddToRoleAsync(updatedUser, fetchedRole);
            }

            AppUser completedUser = await _userRepository.GetUserByIdAsync(updatedUser.Id);
            return completedUser;
        }


        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            AppUser fetchedUser = await _userRepository.GetUserByIdAsync(userId);
            if (fetchedUser == null) throw new Exception($"Could not find user with Id : {userId}");
            EmailToken verifyToken = await _userRepository.GetTokenByUserId(userId);
            if (verifyToken.Token != token)
                throw new Exception($"Could not confirm email with Id : {userId}");
            if (verifyToken.CreatedAt < DateTime.Now.AddMinutes(15) && verifyToken.IsUsed == false &&
                verifyToken.UserId == userId)
            {
                bool activatedUser = await _userRepository.SetActiveAsync(userId);
                if (activatedUser) return true;
            }

            throw new Exception($"Could not confirm email with user Id : {userId}");
        }

        public Task<bool> ChangePasswordAsync(AppUser user, string newPassword)
        {
            // find user => compare if password matched the current password => if yes then generate and save new password
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _userRepository.DeleteUser(id);
        }
    }
}