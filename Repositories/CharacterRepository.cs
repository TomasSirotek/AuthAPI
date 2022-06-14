using Data_Access.Data;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories; 

public class CharacterRepository : ICharacterRepository{
    
    private readonly Context _context;

    public CharacterRepository(Context context)
    {
        _context = context;
    }
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetAsync()
    {
        return await _context.Characters.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> CreateAsync(Product character)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> UpdateAsync(Product character)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
}