using DatabaseLabes.SharedKernel.Shared;
using First_10.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace First_10.DataAccess.Configurations
{
       public class StockAvailabilityConfiguration : IEntityTypeConfiguration<StockAvailability>
       {
              #region Implementation of IEntityTypeConfiguration<StockAvailability>

              /// <inheritdoc />
              public void Configure(EntityTypeBuilder<StockAvailability> builder)
              {
                     builder.ToTable("StockAvailability_1");
                     builder.HasKey(x => x.Id);

                     builder.Property(x => x.Id)
                            .HasColumnName("Id")
                            .HasColumnType("int")
                            .IsRequired();

                     builder.Property(p => p.ProductId)
                            .HasColumnName("ProductId")
                            .HasColumnType("int")
                            .IsRequired();

                     builder.Property(p => p.Price)
                            .HasColumnName("Price")
                            .HasColumnType("int")
                            .IsRequired();

                     builder.Property(p => p.WarehouseCount)
                            .HasColumnName("WarehouseCount")
                            .HasColumnType("int")
                            .IsRequired();

                     builder.Property(p => p.Discount)
                            .HasColumnName("Discount")
                            .HasColumnType("int")
                            .IsRequired(false);

                     builder.Property(p => p.Availability)
                            .HasColumnName("Availability")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(512)
                            .IsRequired(false);

                     builder.AddSoftDeleteColumns();

                     builder.HasOne(x => x.Product)
                            .WithMany(x => x.StockAvailabilities)
                            .HasForeignKey(x => x.ProductId)
                            .IsRequired();
              }

              #endregion
       }
}
