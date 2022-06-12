using Data_Access.Models;

namespace Data_Access.Data; 

public class DataSeeder {

    private readonly Context _context;
    
    public DataSeeder(Context context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Characters.Any())
        {
            List<Character> characters = new List<Character>()
            {
                new Character()
                {
                    Id = Guid.NewGuid().ToString(),
                    FullName = "Michael De Santa",
                    Status = true,
                    KnownFor = "GTA",
                    Gender = "Male",
                    DoB = DateTime.Now,
                    Nationality = "American"
                },
                new Character()
                {
                    Id = Guid.NewGuid().ToString(),
                    FullName = "Franklin Clinton ",
                    Status = true,
                    KnownFor = "GTA",
                    Gender = "Male",
                    DoB = DateTime.Now,
                    Nationality = "American"
                }
            };
            _context.Characters.AddRangeAsync(characters);
            _context.SaveChangesAsync();
        }

    }
    
}