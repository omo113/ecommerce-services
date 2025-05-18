using CatalogService.Domain.Aggregates.ProductEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Infrastructure.Persistance.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(c => c.Id);
        builder.HasAlternateKey(x => x.UId);
        builder.Property(x => x.UId).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(c => c.Name)
            .HasMaxLength(64);


        builder.OwnsOne(c => c.Image, image =>
        {
            image.HasKey(i => i.Id);
        });
        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId);
    }
}