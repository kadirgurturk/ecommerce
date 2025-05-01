using basic_ecommerce.Domain;
using Microsoft.EntityFrameworkCore;

namespace basic_ecommerce.Infrastructure;

public class EcommerceContext : DbContext
{
    public EcommerceContext(DbContextOptions<EcommerceContext> options)
        : base(options) 
    { 
    }
    
    public DbSet<User> Users { get; set; } 
    public DbSet<Product> Products { get; set; } 
    public DbSet<Order> Orders { get; set; } 
    public DbSet<OrderItem> OrderItems { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<OrderItem>()
            .HasOne(o => o.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(o => o.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(o => o.ProductId);
    }
}