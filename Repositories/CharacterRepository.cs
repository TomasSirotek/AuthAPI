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
    
    public async Task<List<Character>> GetAsyncItemById()
    {
        // TODO: Work on implementing sql queries for all of them 
        return null;
    }

    public async Task<List<Character>> GetAsync()
    {
        return await _context.Characters.ToListAsync();
    }

    public async Task<Character> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Character> CreateAsync(Character character)
    {
        throw new NotImplementedException();
    }

    public async Task<Character> UpdateAsync(Character character)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
}