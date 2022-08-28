using ProductAPI.Identity.Models;
using ProductAPI.Infrastructure.Repositories.Interfaces;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Services; 

public class RoleService : IRoleService {

    private readonly IRoleRepository _roleRepository;
    
    public RoleService (IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    public async Task<List<UserRole>> GetAsync()
    {
        return await _roleRepository.GetRoleAsync();
    }

    public async Task<UserRole> GetAsyncById(string id)
    {
         return await _roleRepository.GetRoleByIdAsync(id);
    }

    public async Task<UserRole> GetAsyncByName(string name)
    {
        return await _roleRepository.GetRoleAsyncByName(name);
    }

    public async Task<UserRole> CreateAsync(UserRole role)
    {
        bool result  = await _roleRepository.CreateRoleAsync(role);
        if (!result)  throw new Exception("Could not create role");
        UserRole fetchedRole = await _roleRepository.GetRoleByIdAsync(role.Id);
        return fetchedRole;
    }

    public async Task<UserRole> UpdateAsync(UserRole role)
    {
        bool result = await _roleRepository.UpdateRoleAsync(role);
        if (!result) 
            throw new Exception("Role could not be created");
        UserRole updatedRole = await _roleRepository.GetRoleByIdAsync(role.Id);
        return updatedRole;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _roleRepository.DeleteAsync(id);
    }
}