using ClassLibraryModel;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Products> Products => Set<Products>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Products>(entity =>
        {
            entity.ToTable("products", "public");
            entity.HasKey(p => p.ProductId);

            entity.Property(p => p.ProductId)
                .HasColumnName("product_id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(p => p.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            entity.Property(p => p.Price)
                .HasColumnName("price")
                .HasPrecision(10, 2);

            entity.Property(p => p.StockQuantity)
                .HasColumnName("stock_quantity")
                .HasDefaultValue(0);

            entity.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");
        });
    }
}