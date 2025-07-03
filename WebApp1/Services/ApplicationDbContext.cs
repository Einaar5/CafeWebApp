using Microsoft.EntityFrameworkCore;
using WebApp1.Models;   

namespace WebApp1.Services
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Your DbSets...
        public DbSet<Content> Contents { get; set; } = null!;

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Gallery> Galleries { get; set; } = null!;

        public DbSet<CategoryMenu> CategoryMenus { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Content>().ToTable("Contents");

            // İlişki tanımı: bir Category, birden çok Product'a sahip olabilir
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

        }

    }
}

