using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Domain.Models;
using MediatR;

namespace ProductAPI.Infrastructure.Data {
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : 
            base(options)
        { }
        public DbSet<Product> Characters { get; set; }
        public DbSet<Category> Category { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                         e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(255)");
            
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
        }
    }

    public class Event : Message,INotification{
        
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }

    public abstract class Message {
        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}