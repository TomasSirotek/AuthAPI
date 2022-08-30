using Dapper;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Domain.Models;
using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories.Interfaces;

namespace ProductAPI.Infrastructure.Repositories {
    public class UserRepository : IUserRepository {
        private readonly SqlServerConnection _connection;

        public UserRepository(SqlServerConnection connection)
        {
            _connection = connection;
        }

        #region GET

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            using var cnn = _connection.CreateConnection();
            var sql = @"SELECT *
                        FROM app_user u
                        LEFT JOIN user_role ur ON u.id = ur.userId 
                        LEFT JOIN role r ON ur.roleId = r.id";
            
            Dictionary<string, AppUser> userRoles = new Dictionary<string, AppUser>();

            await cnn.QueryAsync<AppUser, UserRole, AppUser>(sql, (u, r) =>
                    {
                        if (!userRoles.TryGetValue(u.Id, out var userEntry))
                        {
                            userEntry = u;
                            userEntry.Roles = new List<UserRole>();
                            userRoles.Add(u.Id, userEntry);
                        }
                        
                        userEntry.Roles.Add(r);
                        return userEntry;
                    });
            return userRoles.Values.ToList();
        }

        public Task<AppUser> GetUserByIdAsync(string id)
        {
            using var cnn = _connection.CreateConnection();
            var sql = @"SELECT *
                        FROM app_user u
                        LEFT JOIN user_role ur ON u.id = ur.userId 
                        LEFT JOIN role r ON ur.roleId = r.id
                        where u.id = @id";

            IEnumerable<AppUser> user = cnn.Query<AppUser, UserRole, AppUser>(sql, (u, r) =>
                    {
                        var userRoles = new Dictionary<string, AppUser>();
                        if (!userRoles.TryGetValue(u.Id, out var user))
                        {
                            userRoles.Add(u.Id, user = u);
                        }
                        
                        user.Roles = new List<UserRole>();
                        user.Roles.Add(r);
                        return user;
                    },
                    new {Id = id}
                ).GroupBy(u => u.Id)
                .Select(group =>
                {
                    AppUser user = group.First();
                    user.Roles = group.Select(u => u.Roles.Single()).ToList();
                    return user;
                });
            return Task.FromResult(user.First());
        }

        public Task<AppUser> GetAsyncByEmailAsync(string email)
        {
            using var cnn = _connection.CreateConnection();
            var sql = @"SELECT *
                        FROM app_user u
                        LEFT JOIN user_role ur ON u.id = ur.userId 
                        LEFT JOIN role r ON ur.roleId = r.id
                        where u.email = @email";

            IEnumerable<AppUser> user = cnn.Query<AppUser, UserRole, AppUser>(sql, (u, r) =>
                    {
                        var userRoles = new Dictionary<string, AppUser>();
                        if (!userRoles.TryGetValue(u.Id, out var user))
                        {
                            userRoles.Add(u.Id, user = u);
                        }

                        user.Roles.Add(r);
                        return user;
                    },
                    new {Email = email}
                ).GroupBy(u => u.Id)
                .Select(group =>
                {
                    AppUser user = @group.First();
                    user.Roles = @group.Select(u => u.Roles.Single()).ToList();
                    return user;
                });
            AppUser[] appUsers = user as AppUser[] ?? user.ToArray();
            return Task.FromResult(appUsers.First());
        }

        public async Task<RefreshToken> FindByTokenAsync(string token)
        {
            using var cnn = _connection.CreateConnection();
            return await cnn.QuerySingleOrDefaultAsync<RefreshToken>( 
                @"SELECT * FROM refreshToken AS r WHERE r.token = @token", new {Token = token});
        }

        public async Task<EmailToken> GetTokenByUserId(string id)
        {
            using var cnn = _connection.CreateConnection();
            return await cnn.QuerySingleOrDefaultAsync<EmailToken>(
                $@"SELECT * FROM email_token AS et WHERE et.userId = @Id", new {Id = id});
        }

        #endregion
        
        #region POST

        public async Task<AppUser> CreateUserAsync(AppUser user)
        {
            using var cnn = _connection.CreateConnection();

            var newUser = await cnn.ExecuteAsync(  
                $@"INSERT INTO app_user (id,firstName,lastName,email,passwordHash,isActivated,createdAt) 
                                        VALUES (@id,@firstName,@lastName,@email,@passwordHash,@isActivated,@createdAt)", user);
            if (newUser > 0)
                return user;
            throw new ArgumentNullException(nameof(user));
        }

        public async Task<AppUser> AddToRoleAsync(AppUser user, UserRole role)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = @"insert into user_role (userId,roleId) 
                        values (@userId,@roleId)";

                var newUser = await cnn.ExecuteAsync(sql, new
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
                if (newUser > 0)
                    return user;
            }

            throw new ArgumentNullException(nameof(user));
        }

        public async Task<EmailToken> CreateEmailToken(string userId,EmailToken newToken)
        {
            using var cnn = _connection.CreateConnection();
            var affectedRows = await cnn.ExecuteAsync(
                $@"INSERT INTO email_token (id,userId,token,createdAt,isUsed) 
                    VALUES (@id,@userId,@token,@createdAt,@isUsed)", new
            {
                id = newToken.Id,
                userId = userId,
                token = newToken.Token,
                createdAt = newToken.CreatedAt,
                isUsed = newToken.IsUsed
            });
            
            if (affectedRows > 0)
                return newToken;
            return null;
        }

        #endregion

        #region PUT

        public async Task<bool> UpdateToken(RefreshToken refreshToken)
        {
            using var cnn = _connection.CreateConnection();
            var affectedRows = await cnn.ExecuteAsync( 
                $@"INSERT INTO refreshToken (id,userId,token,JwtId,isUsed,isRevoked,AddedDate,ExpDate) 
                        values (@id,@userId,@token,@JwtId,@isUsed,@isRevoked,@addedDate,@expDate)", 
                refreshToken);
            return affectedRows > 0;
        }

        public async Task<AppUser> UpdateAsync(AppUser user)
        {
            using var cnn = _connection.CreateConnection();
            var affectedRows = await cnn.ExecuteAsync( 
                $@"UPDATE
                        app_user
                        SET 
                        firstname = @firstname,
                        lastName = @lastName,
                        email = @email,
                        isActivated = @isActivated,
                        updatedAt = @updatedAt
                        WHERE id = @id;", new
            {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                isActivated = user.IsActivated,
                updatedAt = user.UpdatedAt
            });
            if (affectedRows > 0)
                return user;

            return null;
        }
        
        public async Task<bool> ChangePasswordAsync(AppUser user, string newPasswordHash)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetActiveAsync(string id)
        {
            using var cnn = _connection.CreateConnection();
            var affectedRows = await cnn.ExecuteAsync(
                $@"UPDATE
                        app_user
                        SET 
                        isActivated = 1
                        WHERE id = @Id", new {Id = id});
            return affectedRows > 0;
        }

        #endregion

        #region DELETE

        public async Task<bool> RemoveUserRoleAsync(string roleId)
        {
            using var cnn = _connection.CreateConnection();
            var affectedRows = await cnn.ExecuteAsync( 
                $@"DELETE FROM user_role  WHERE roleId = @roleId", new { RoleId = roleId});

            return affectedRows > 0;
        }

        public async Task<bool> DeleteUser(string id)
        {
            using var cnn = _connection.CreateConnection();
            var affectedRows = await cnn.ExecuteAsync(
                $@"DELETE 
                         FROM app_user 
                         WHERE id = @Id", new {Id = id});
            return affectedRows > 0;
        }

        #endregion
    }
}