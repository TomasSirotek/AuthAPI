using ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Data {
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : 
            base(options)
        {
        }
        public DbSet<Product> Characters => Set<Product>();
        public DbSet<Category> Category => Set<Category>();
    }
}