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

    public async Task<List<Character>> GetAsync()
    {
        return await _context.Characters.ToListAsync();
    }

    public async Task<Character> GetByIdAsync(string id)
    { 
        return await _context.Characters.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(Character character)
    {
        await _context.AddAsync(character);
    }

    public async Task<Character> UpdateAsync(Character character)
    {
        throw new NotImplementedException();
    }

    public void DeleteAsync(Character character)
    {
        _context.Characters.Remove(character);
    }
}