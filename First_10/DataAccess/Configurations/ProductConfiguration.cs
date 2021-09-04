using DatabaseLabes.SharedKernel.Shared;
using First_10.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace First_10.DataAccess.Configurations
{
       public class ProductConfiguration : IEntityTypeConfiguration<Product>
       {
              #region Implementation of IEntityTypeConfiguration<Product>

              /// <inheritdoc />
              public void Configure(EntityTypeBuilder<Product> builder)
              {
                     builder.ToTable("Products_1");
                     builder.HasKey(x => x.Id);

                     builder.Property(p => p.Id)
                            .HasColumnName("Id")
                            .HasColumnType("int")
                            .IsRequired();

                     builder.Property(p => p.Title)
                            .HasColumnName("Title")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(512)
                            .IsRequired(false);

                     builder.Property(p => p.Producer)
                            .HasColumnName("Producer")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(512)
                            .IsRequired(false);

                     builder.Property(p => p.Category)
                            .HasColumnName("Category")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(512)
                            .IsRequired(false);

                     builder.Property(p => p.Description)
                            .HasColumnName("Description")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(1024)
                            .IsRequired(false);

                     builder.Property(p => p.Photo)
                            .HasColumnName("Photo")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(512)
                            .IsRequired(false);

                     builder.Property(p => p.Availability)
                            .HasColumnName("Availability")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(512)
                            .IsRequired(false);

                     builder.AddSoftDeleteColumns();

                     builder.HasMany(x => x.StockAvailabilities)
                            .WithOne(x => x.Product)
                            .HasForeignKey(x => x.ProductId)
                            .IsRequired();

                     builder.HasMany(x => x.Sells)
                            .WithOne(x => x.Product)
                            .HasForeignKey(x => x.ProductId)
                            .IsRequired()
                            .OnDelete(DeleteBehavior.NoAction);
              }

              #endregion
       }
}
