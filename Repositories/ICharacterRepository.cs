using Data_Access.Models;

namespace Data_Access.Repositories; 

public interface ICharacterRepository {
    Task SaveAsync();
    
    Task<List<Character>> GetAsync();
    
    Task<Character> GetByIdAsync(string id);

    Task<Character> CreateAsync(Character character);
        
    Task<Character> UpdateAsync(Character character);
        
    Task<bool> DeleteAsync(string id);
}