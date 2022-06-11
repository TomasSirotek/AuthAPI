using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Data {
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : 
            base(options)
        {
        }

        public DbSet<Character> Characters => Set<Character>();
    }
}