using Data_Access.Models;

namespace Data_Access.Repositories; 

public interface ICharacterRepository {
    Task SaveAsync();
    
    Task<List<Character>> GetAsync();
    
    Task<Character> GetByIdAsync(string id);

    Task CreateAsync(Character character);
        
    Task<Character> UpdateAsync(Character character);

    void DeleteAsync(Character character);
}