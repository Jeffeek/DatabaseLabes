using First_10.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace First_10.DataAccess.Configurations
{
       public class SellConfiguration : IEntityTypeConfiguration<Sell>
       {
              #region Implementation of IEntityTypeConfiguration<Sell>

              /// <inheritdoc />
              public void Configure(EntityTypeBuilder<Sell> builder)
              {
                     builder.ToTable("Sells_1");
                     builder.HasKey(x => x.Id);

                     builder.Property(p => p.Id)
                            .HasColumnName("Id")
                            .HasColumnType("int")
                            .IsRequired();

                     builder.Property(p => p.ProductId)
                            .HasColumnName("ProductId")
                            .HasColumnType("int")
                            .IsRequired();

                     builder.Property(p => p.Size)
                            .HasColumnName("Size")
                            .HasColumnType("nvarchar")
                            .HasMaxLength(128)
                            .IsRequired();

                     builder.Property(p => p.SellDate)
                            .HasColumnName("SellDate")
                            .HasColumnType("datetime")
                            .IsRequired();

                     builder.Property(p => p.Count)
                            .HasColumnName("Count")
                            .HasColumnType("int")
                            .IsRequired();

                     builder.HasOne(x => x.Product)
                            .WithMany(x => x.Sells)
                            .HasForeignKey(x => x.ProductId)
                            .IsRequired()
                            .OnDelete(DeleteBehavior.ClientNoAction);
              }

              #endregion
       }
}
