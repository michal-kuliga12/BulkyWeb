using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;
namespace BulkyWeb.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Action", DisplayOrder = 1 },
            new Category { CategoryId = 2, Name = "Action1", DisplayOrder = 2 },
            new Category { CategoryId = 3, Name = "Action2", DisplayOrder = 3 },
            new Category { CategoryId = 4, Name = "Action3", DisplayOrder = 4 }
            );
    }
}
