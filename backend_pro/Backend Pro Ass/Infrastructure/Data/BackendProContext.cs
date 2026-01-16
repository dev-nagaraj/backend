using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class BackendProContext : DbContext
{
    public BackendProContext()
    {
    }

    public BackendProContext(DbContextOptions<BackendProContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DetailsRaw> DetailsRaws { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=backend_pro;Username=postgres;Password=Appa@1028");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerAddress).HasColumnName("customer_address");
            entity.Property(e => e.CustomerEmail).HasColumnName("customer_email");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.Region).HasColumnName("region");
        });

        modelBuilder.Entity<DetailsRaw>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("details_raw");

            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.CustomerAddress).HasColumnName("customer_address");
            entity.Property(e => e.CustomerEmail).HasColumnName("customer_email");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.DateOfSale)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_sale");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.QuantitySold).HasColumnName("quantity_sold");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.ShippingCost).HasColumnName("shipping_cost");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("order_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DateOfSale)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_sale");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.QuantitySold)
                .HasDefaultValue(1)
                .HasColumnName("quantity_sold");
            entity.Property(e => e.ShippingCost).HasColumnName("shipping_cost");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_customer_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product_Id");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
