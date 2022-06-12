using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Data {
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : 
            base(options)
        {
        }

      //  public DbSet<Character> Characters => Set<Character>();
      public DbSet<Character> Characters { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>().HasData(new Character[] {
                new Character
                {
                    Id="4",
                    FullName= "Tony Stark",
                    Status = false,
                    KnownFor = "Iron man",
                    Gender = "Female",
                    DoB = DateTime.Now,
                    Nationality = "American",
                    
                },
                new Character
                {
                    Id="5",
                    FullName= "Tony Dark",
                    Status = false,
                    KnownFor = "Spider man",
                    Gender = "Trans",
                    DoB = DateTime.Now,
                    Nationality = "American psycho",
                    
                },
            });
            
            
        }
    }
}