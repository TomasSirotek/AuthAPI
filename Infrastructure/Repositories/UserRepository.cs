using AuthAPI.Domain.Models;
using AuthAPI.Identity.Models;
using AuthAPI.Infrastructure.Data;
using AuthAPI.Infrastructure.Repositories.Interfaces;
using Dapper;

namespace AuthAPI.Infrastructure.Repositories {
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
            var sql = @"SELECT DISTINCT *
                            FROM app_user u
                            INNER JOIN user_role ur ON u.id = ur.userId 
                            INNER JOIN role r ON ur.roleId = r.id
                            LEFT JOIN address a ON a.userId = u.id";
            
            Dictionary<string, AppUser> userRoles = new Dictionary<string, AppUser>();
            IEnumerable<AppUser> users = await cnn.QueryAsync<AppUser, UserRole, Address, AppUser>(sql, (u, r,a) =>
                    { 
                        if (!userRoles.TryGetValue(u.Id, out var userEntry))
                        {
                            userEntry = u;
                            userEntry.Roles = new List<UserRole>();
                            userRoles.Add(u.Id, userEntry);
                        }
                        
                        if (r == null)  userEntry.Roles = new List<UserRole>();
                        if (a == null) userEntry.Address = new Address();
                        if (r != null) userEntry.Roles.Add(r);
                        if (a != null) userEntry.Address = a;

                        return userEntry;
                    },splitOn:"id");

            List<AppUser> appUsers = users.Distinct().ToList();
            return appUsers.Any() ? appUsers.ToList() : null!;
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            using var cnn = _connection.CreateConnection();
            var sql = @"SELECT *
                        FROM app_user u
                        INNER JOIN user_role ur ON u.id = ur.userId 
                        INNER JOIN role r ON ur.roleId = r.id
                        LEFT JOIN address a ON a.userId = u.id
                        where u.id = @id";
            
            Dictionary<string, AppUser> userRoles = new Dictionary<string, AppUser>();
            IEnumerable<AppUser> user = await cnn.QueryAsync<AppUser, UserRole, Address, AppUser>(sql, (u, r,a) =>
            {
                if (!userRoles.TryGetValue(u.Id, out var userEntry))
                {
                    userEntry = u;
                    userEntry.Roles = new List<UserRole>();
                    userRoles.Add(u.Id, userEntry);
                }
                
                if (r == null)  userEntry.Roles = new List<UserRole>();
                if (a == null) userEntry.Address = new Address();
                if (r != null) userEntry.Roles.Add(r);
                if (a != null) userEntry.Address = a;
                return userEntry;
                
            },new {Id = id}, splitOn:"id");
            List<AppUser> appUsers = user.Distinct().ToList();
            return appUsers.Any() ? appUsers.FirstOrDefault()! : null!;
        }

        public async Task<AppUser> GetAsyncByEmailAsync(string email)
        {
            using var cnn = _connection.CreateConnection();
            var sql = @"SELECT *
                        FROM app_user u
                        INNER JOIN user_role ur ON u.id = ur.userId 
                        INNER JOIN role r ON ur.roleId = r.id
                        LEFT JOIN address a ON a.userId = u.id
                        where u.email = @email";

            Dictionary<string, AppUser> userRoles = new Dictionary<string, AppUser>();
            IEnumerable<AppUser> user = await cnn.QueryAsync<AppUser, UserRole, Address,AppUser>(sql, (u, r,a) =>
            {
                if (!userRoles.TryGetValue(u.Id, out var userEntry))
                {
                    userEntry = u;
                    userEntry.Roles = new List<UserRole>();
                    userRoles.Add(u.Id, userEntry);
                }
                        
                if (r == null)  userEntry.Roles = new List<UserRole>();
                if (a == null) userEntry.Address = new Address();
                if (r != null) userEntry.Roles.Add(r);
                if (a != null) userEntry.Address = a;
                return userEntry;
            }, new {Email = email });

            List<AppUser> appUsers = user.Distinct().ToList();
            return appUsers.Any() ? appUsers.FirstOrDefault()! : null!;
        }

        public async Task<RefreshToken> FindByTokenAsync(string token)
        {
            using var cnn = _connection.CreateConnection();
            return await cnn.QuerySingleOrDefaultAsync<RefreshToken>( 
                @"SELECT * FROM refresh_token AS r WHERE r.token = @token", new {Token = token});
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
            return null!;
        }

        public async Task<AppUser> AddToRoleAsync(AppUser user, UserRole role)
        {
            using var cnn = _connection.CreateConnection();
            var sql = @"insert into user_role (userId,roleId) 
                        values (@userId,@roleId)";

            var newUser = await cnn.ExecuteAsync(sql, new
            {
                UserId = user.Id,
                RoleId = role.Id
            });
            if (newUser > 0)
                return user;

            return null!;
        }
        
        public async Task<Address> AddAddressToUser(string userId,Address address)
        {
            using var cnn = _connection.CreateConnection();
            var sql = @"insert into address (id,userId,street,number,country,zip) 
                        values (@id,@userId,@street,@number,@country,@zip)";

            var rowsAffected = await cnn.ExecuteAsync(sql, new
            {
                Id = address.Id,
                UserId = userId,
                Street = address.Street,
                Number = address.Number,
                Country = address.Country,
                Zip = address.Zip
            });
            if (rowsAffected > 0)
                return address;

            return null!;
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
            return null!;
        }

        #endregion

        #region PUT

        public async Task<bool> UpdateToken(RefreshToken refreshToken)
        {
            using var cnn = _connection.CreateConnection();
            var affectedRows = await cnn.ExecuteAsync( 
                $@"INSERT INTO refresh_token (id,userId,token,isUsed,isRevoked,AddedDate,ExpDate) 
                        values (@id,@userId,@token,@isUsed,@isRevoked,@addedDate,@expDate)", 
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

            return null!;
        }
        
        public async Task<bool> ChangePasswordAsync(string userId, string newPasswordHash)
        {
            using var cnn = _connection.CreateConnection();
            var affectedRows = await cnn.ExecuteAsync( 
                $@"UPDATE
                        app_user
                        SET 
                        passwordHash = @passwordHash
                        WHERE id = @id;", new
                {
                    id = userId,
                    passwordHash = newPasswordHash
                });
            if (affectedRows > 0)
                return true;
            return false;
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