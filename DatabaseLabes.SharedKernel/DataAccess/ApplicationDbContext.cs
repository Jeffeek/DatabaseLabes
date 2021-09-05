using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DatabaseLabes.SharedKernel.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLabes.SharedKernel.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            if (options.Extensions.FirstOrDefault(x => x.Info.IsDatabaseProvider) != null)
                Database.EnsureCreated();
        }

        /// <inheritdoc />
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var product in ChangeTracker.Entries<ISoftDelete>()
                                                 .Where(product => product.State == EntityState.Deleted))
            {
                product.Entity.IsDeleted = true;
                product.Entity.Deleted = DateTime.Now;
                product.State = EntityState.Modified;
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override int SaveChanges()
        {
            foreach (var product in ChangeTracker.Entries<ISoftDelete>()
                                                 .Where(product => product.State == EntityState.Deleted))
            {
                product.Entity.IsDeleted = true;
                product.Entity.Deleted = DateTime.Now;
                product.State = EntityState.Modified;
            }

            return base.SaveChanges();
        }

        #region Overrides of DbContext

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetEntryAssembly());
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
