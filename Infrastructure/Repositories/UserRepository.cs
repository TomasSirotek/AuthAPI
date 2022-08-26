using Dapper;
using Microsoft.IdentityModel.Tokens;
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
            using (var cnn = _connection.CreateConnection())
            {
                var sql = @"SELECT *
                        FROM app_user u
                        LEFT JOIN user_role ur ON u.id = ur.userId 
                        LEFT JOIN role r ON ur.roleId = r.id";

                IEnumerable<AppUser> users = cnn.Query<AppUser, UserRole, AppUser>(sql, (u, r) =>
                        {
                            Dictionary<string, AppUser> userRoles = new Dictionary<string, AppUser>();
                            AppUser user;
                            if (!userRoles.TryGetValue(u.Id, out user))
                            {
                                userRoles.Add(u.Id, user = u);
                            }

                            if (user.Roles == null)
                                user.Roles = new List<UserRole>();
                            user.Roles.Add(r);
                            return user;
                        },
                        splitOn: "id"
                    ).GroupBy(u => u.Id)
                    .Select(group =>
                    {
                        AppUser user = group.First();
                        user.Roles = group.Select(u => u.Roles.Single()).ToList();
                        return user;
                    });
                return users.ToList();
            }
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = @"SELECT *
                        FROM app_user u
                        LEFT JOIN user_role ur ON u.id = ur.userId 
                        LEFT JOIN role r ON ur.roleId = r.id
                        where u.id = @id";

                IEnumerable<AppUser> user = cnn.Query<AppUser, UserRole, AppUser>(sql, (u, r) =>
                        {
                            var userRoles = new Dictionary<string, AppUser>();
                            AppUser user;
                            if (!userRoles.TryGetValue(u.Id, out user))
                            {
                                userRoles.Add(u.Id, user = u);
                            }

                            if (user.Roles == null)
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
                return user.First();
            }
        }

        public async Task<AppUser> GetAsyncByEmailAsync(string email)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = @"SELECT *
                        FROM app_user u
                        LEFT JOIN user_role ur ON u.id = ur.userId 
                        LEFT JOIN role r ON ur.roleId = r.id
                        where u.email = @email";

                IEnumerable<AppUser> user = cnn.Query<AppUser, UserRole, AppUser>(sql, (u, r) =>
                        {
                            var userRoles = new Dictionary<string, AppUser>();
                            AppUser user;
                            if (!userRoles.TryGetValue(u.Id, out user))
                            {
                                userRoles.Add(u.Id, user = u);
                            }

                            if (user.Roles == null)
                                user.Roles = new List<UserRole>();
                            user.Roles.Add(r);
                            return user;
                        },
                        new {Email = email}
                    ).GroupBy(u => u.Id)
                    .Select(group =>
                    {
                        AppUser user = group.First();
                        user.Roles = group.Select(u => u.Roles.Single()).ToList();
                        return user;
                    });
                AppUser[] appUsers = user as AppUser[] ?? user.ToArray();
                if (appUsers.Any())
                    return appUsers.First();
                return null;
            }
        }

        public async Task<RefreshToken> FindByTokenAsync(string token)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = @"select * from refreshToken as r where r.token = @token";

                RefreshToken newToken = await cnn.QueryFirstAsync<RefreshToken>(sql, new {Token = token});
                if (token != null)
                {
                    return newToken;
                }

                throw new ArgumentNullException(nameof(token));
            }
        }

        public async Task<EmailToken> GetTokenByUserId(string id)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = $@"select * from email_token as et where et.userId = @Id";

                EmailToken emailTokenResult = cnn.QueryFirstOrDefault<EmailToken>(sql, new
                {
                    Id = id
                });
                if (emailTokenResult != null)
                    return emailTokenResult;
                return null;
            }
        }

        #endregion


        #region POST

        public async Task<AppUser> CreateUserAsync(AppUser user)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql =
                    $@"INSERT INTO app_user (id,firstName,lastName,email,passwordHash,isActivated,createdAt) 
                                        values (@id,@firstName,@lastName,@email,@passwordHash,@isActivated,@createdAt)";

                var newUser = await cnn.ExecuteAsync(sql, user);
                if (newUser > 0)
                    return user;
                throw new ArgumentNullException(nameof(user));
            }
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
            using (var cnn = _connection.CreateConnection())
            {
                var sql =
                    $@"INSERT INTO email_token (id,userId,token,createdAt,isUsed) 
                                        values (@id,@userId,@token,@createdAt,@isUsed)";

                var affectedRows = await cnn.ExecuteAsync(sql, new
                {
                    id = newToken.Id,
                    userId = userId,
                    token = newToken.Token,
                    createdAt = newToken.CreatedAt,
                    isUsed = newToken.IsUsed
                });
                if (affectedRows > 0)
                    return newToken;
            }
            throw new ArgumentNullException(nameof(userId));
        }

        #endregion

        #region PUT

        public async Task<RefreshToken> UpdateToken(RefreshToken refreshToken)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql =
                    $@"INSERT INTO refreshToken (id,userId,token,JwtId,isUsed,isRevoked,AddedDate,ExpDate) 
                                        values (@id,@userId,@token,@JwtId,@isUsed,@isRevoked,@addedDate,@expDate)";

                var newUser = await cnn.ExecuteAsync(sql, refreshToken);
                if (newUser > 0)
                    return refreshToken;
                return null;
            }
        }

        public async Task<AppUser> UpdateAsync(AppUser user)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = $@"update
                        app_user
                        set 
                        firstname = @firstname,
                        lastName = @lastName,
                        email = @email,
                        isActivated = @isActivated,
                        updatedAt = @updatedAt
                        where id = @id;";

                var newUser = await cnn.ExecuteAsync(sql, new
                {
                    id = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    isActivated = user.IsActivated,
                    updatedAt = user.UpdatedAt
                });
                if (newUser > 0)
                    return user;
            }

            return null;
        }


        public async Task<bool> ChangePasswordAsync(AppUser user, string newPasswordHash)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetActiveAsync(string id)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = $@"update
                        app_user
                        set 
                        isActivated = 1
                        where id = @Id";

                var affectedRows = await cnn.ExecuteAsync(sql, new
                {
                    Id = id
                });
                if (affectedRows > 0)
                    return true;
                return false;
            }
        }

        #endregion

        #region DELETE

        public async Task<bool> RemoveUserRoleAsync(string roleId)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql =
                    $@"DELETE FROM user_role  WHERE roleId = @roleId
                                ";

                var affectedRows = await cnn.ExecuteAsync(sql, new
                {
                    RoleId = roleId
                });
                if (affectedRows > 0)
                    return true;
            }

            throw new ArgumentNullException(nameof(roleId));
        }

        public async Task<bool> DeleteUser(string id)
        {
            using (var cnn = _connection.CreateConnection())
            {
                var sql = $@"Delete 
                         from app_user 
                         where id = @Id";

                var newUser = await cnn.ExecuteAsync(sql, new
                {
                    Id = id
                });
                if (newUser > 0)
                    return true;
                return false;
            }
        }

        #endregion
    }
}