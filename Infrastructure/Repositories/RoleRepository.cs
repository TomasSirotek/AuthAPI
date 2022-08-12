using Dapper;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories.Interfaces;

namespace ProductAPI.Infrastructure.Repositories; 

public class RoleRepository : IRoleRepository{
    
    private readonly SqlServerConnection _connection;

    public RoleRepository(SqlServerConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<List<UserRole>> GetRoleAsync()
    {
        using (var cnn = _connection.CreateConnection())
        {
            var sql = @"select * from role";

            IEnumerable<UserRole> newRoles = await cnn.QueryAsync<UserRole>(sql);
            if (!newRoles.IsNullOrEmpty())
            {
                return newRoles.ToList();
            }
            throw new ArgumentNullException(nameof(newRoles));
        }
    }

    public async Task<UserRole> GetRoleAsyncByName(string name)
    {
        using (var cnn = _connection.CreateConnection())
        {
            var sql = @"select * from role as c where c.name = @name";

            UserRole role = await cnn.QueryFirstAsync<UserRole>(sql, new {Name = name});
            if (role != null)
            {
                return role;
            }
            throw new ArgumentNullException(nameof(role));
        }
    }

    public async Task<UserRole> GetRoleByIdAsync(string id)
    {
        using (var cnn = _connection.CreateConnection())
        {
            var sql = @"select * from role as c where c.id = @id";

            UserRole role = await cnn.QueryFirstAsync<UserRole>(sql, new {Id = id});
            if (role != null)
            {
                return role;
            }
            throw new ArgumentNullException(nameof(role));
        }
    }

    public async Task<bool> CreateRoleAsync(UserRole role)
    {
        using (var cnn = _connection.CreateConnection())
        {
            var sql = $@"insert into role (id,name) 
                        values (@id,@name)";

            var affectedRows = await cnn.ExecuteAsync(sql, role);
            if (affectedRows > 0)
            {
                return true;
            }
            throw new ArgumentNullException(nameof(role));
        }
    }

    public async Task<UserRole> UpdateRoleAsync(UserRole role)
    {
        using (var cnn = _connection.CreateConnection())
        {
        var sql = $@"update role
                        SET name = @name 
                        where id = @id;";

        var affectedRows = await cnn.ExecuteAsync(sql, new
        {
            id = role.Id,
            name = role.Name
        });
            
        if (affectedRows > 0)
            return role;
        }
        throw new ArgumentNullException(nameof(role));
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using (var cnn = _connection.CreateConnection())
        {
            var sql =
                $@"DELETE 
                   FROM role
                   WHERE id = @id";

            var deletePExecuteAsync = await cnn.ExecuteAsync(sql,new
            {
                Id = id
            });
            if (deletePExecuteAsync > 0)
            {
                return true;
            }
            throw new ArgumentNullException(nameof(id));
        }
    }
}