using AuthAPI.Identity.Models;
using AuthAPI.Infrastructure.Data;
using AuthAPI.Infrastructure.Repositories.Interfaces;
using Dapper;

namespace AuthAPI.Infrastructure.Repositories;

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
                @"SELECT * FROM role");
    }

    public async Task<UserRole> GetRoleAsyncByName(string name)
    {
        using var cnn = _connection.CreateConnection();
        return
            await cnn.QuerySingleOrDefaultAsync<UserRole>(
                @"SELECT * FROM role AS c WHERE c.name = @name", new {Name = name});
    }

    public async Task<UserRole> GetRoleByIdAsync(string id)
    {
        using var cnn = _connection.CreateConnection();
        return await cnn.QuerySingleOrDefaultAsync<UserRole>(
            @"SELECT * FROM role AS c WHERE c.id = @id",
            new {Id = id});
    }

    public async Task<bool> CreateRoleAsync(UserRole role)
    {
        using var cnn = _connection.CreateConnection();
        var affectedRows = await cnn.ExecuteAsync(
            @"INSERT INTO role (id,name) VALUES (@id,@name)",
            role);
        return affectedRows > 0;
    }

    public async Task<bool> UpdateRoleAsync(UserRole role)
    {
        using var cnn = _connection.CreateConnection();
        var affectedRows = await cnn.ExecuteAsync(
            $@"UPDATE role
                         SET name = @name 
                         WHERE id = @id;", role);
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