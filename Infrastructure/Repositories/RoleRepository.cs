using Dapper;
using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories.Interfaces;

namespace ProductAPI.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository {
    private readonly SqlServerConnection _connection;

    public RoleRepository(SqlServerConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<UserRole>> GetRoleAsync()
    {
        using var cnn = _connection.CreateConnection();
        return (List<UserRole>)
            await cnn.QueryAsync<UserRole>(
                @"select * from role");
    }

    public async Task<UserRole> GetRoleAsyncByName(string name)
    {
        using var cnn = _connection.CreateConnection();
        return
            await cnn.QueryFirstAsync<UserRole>(
                @"select * from role as c where c.name = @name", new {Name = name});
    }

    public async Task<UserRole> GetRoleByIdAsync(string id)
    {
        using var cnn = _connection.CreateConnection();
        return await cnn.QuerySingleOrDefaultAsync<UserRole>(
            @"select * from role as c where c.id = @id",
            new {Id = id});
    }

    public async Task<bool> CreateRoleAsync(UserRole role)
    {
        using var cnn = _connection.CreateConnection();
        var affectedRows = await cnn.ExecuteAsync(
            @"insert into role (id,name) values (@id,@name)",
            role);
        return affectedRows > 0;
    }

    public async Task<bool> UpdateRoleAsync(UserRole role)
    {
        using var cnn = _connection.CreateConnection();
        var affectedRows = await cnn.ExecuteAsync(
            $@"update role
                         SET name = @name 
                         where id = @id;", role);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var cnn = _connection.CreateConnection();
        var affectedRows = await cnn.ExecuteAsync(
            $@"DELETE 
                   FROM role
                   WHERE id = @id", new {Id = id});
        return affectedRows > 0;
    }
}