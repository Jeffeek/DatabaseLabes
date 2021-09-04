using System.Linq;
using DatabaseLabes.SharedKernel.DataAccess;
using DatabaseLabes.SharedKernel.DataAccess.Models;
using DatabaseLabes.SharedKernel.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseLabes.SharedKernel.Shared
{
    public static class SharedDbContextOptions
    {
        public static DbContextOptions<ApplicationDbContext> GetInMemoryOptions() =>
            new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("inMem")
                                                               .Options;

        public static DbContextOptions<ApplicationDbContext> GetOptions(string connectionString = "server=localhost;initial catalog=databaseLabs;Trusted_Connection=True;multipleactiveresultsets=True;application name=EntityFramework") =>
            new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString)
                                                               .Options;

        public static EntityTypeBuilder<TEntity> AddSoftDeleteColumns<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, ISoftDelete
        {
            builder.Property(p => p.IsDeleted)
                   .HasColumnName("IsDeleted")
                   .HasColumnType("bit")
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(p => p.Deleted)
                   .HasColumnName("Deleted")
                   .HasColumnType("datetime")
                   .IsRequired(false);

            return builder;
        }

        public static IQueryable<T> ApplyQuery<T>(this IQueryable<T> items, IQuery<T> query) where T : class
        {
            var result = items.Where(query.GetExpression());

            result = query.GetIncludes()
                          .Aggregate(result, (current, include) => current.Include(include));

            return result;
        }
    }
}