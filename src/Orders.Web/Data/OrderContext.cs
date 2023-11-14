using Microsoft.EntityFrameworkCore;
using Orders.Web.Entities;

namespace Orders.Web.Data;

public class OrderContext : DbContext
{
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderLine> OrderLines { get; set; } = null!;
    
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Primary keys
        modelBuilder.Entity<Order>().HasKey(o => o.Id);
        modelBuilder.Entity<OrderLine>().HasKey(ol => ol.Id);
        
        //Properties
        
        //OrderLine ProductPrice
        modelBuilder.Entity<OrderLine>()
            .Property(ol => ol.ProductPrice)
            .HasColumnType("decimal(18,2)");
        
        //Relationships
        modelBuilder.Entity<Order>().ToTable("Order")
            .HasMany(o => o.OrderLines)
            .WithOne(ol => ol.Order)
            .HasForeignKey(ol => ol.OrderId);
        
        modelBuilder.Entity<OrderLine>().ToTable("OrderLine");
    }
}