using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DatabaseLabes.SharedKernel.DataAccess;
using DatabaseLabes.SharedKernel.Service;
using DatabaseLabes.SharedKernel.Shared;
using First_10.DataAccess.Models;
using First_10.Queries;
using Microsoft.EntityFrameworkCore;

namespace First_10.BusinessLogic.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public ProductRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory,
                                 IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Product? GetById(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();

            return db.Set<Product>()
                     .AsNoTracking()
                     .Include(x => x.StockAvailabilities)
                     .Include(x => x.Sells)
                     .FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Product> GetAll()
        {
            using var db = _dbContextFactory.CreateDbContext();

            var products =
                db.Set<Product>()
                  .AsNoTracking()
                  .ApplyQuery(new ProductGetAllQuery())
                  .ToList();

            return products;
        }

        public void Update(Product model)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var dbEntity = db.Set<Product>()
                             .First(x => x.Id == model.Id);

            _mapper.Map(model, dbEntity);

            db.SaveChanges();
        }

        public void Add(Product model)
        {
            model.Id = 0;
            model.StockAvailabilities = new List<StockAvailability>();
            model.Sells = new List<Sell>();
            model.IsDeleted = false;
            model.Deleted = null!;

            using var db = _dbContextFactory.CreateDbContext();

            var existEntity = GetByTitleIncludeDeleted(model.Title);

            if (existEntity != null)
            {
                var id = existEntity.Id;
                _mapper.Map(model, existEntity);

                existEntity.Id = id;
                db.Update(existEntity);
            }
            else
            {
                db.Set<Product>()
                  .Add(model);
            }

            db.SaveChanges();
        }

        public void Remove(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var dbEntity = db.Set<Product>()
                             .Find(id);

            db.Set<Product>()
              .Remove(dbEntity);

            db.SaveChanges();
        }

        internal Product? GetByTitle(string title)
        {
            using var db = _dbContextFactory.CreateDbContext();

            return db.Set<Product>()
                     .AsNoTracking()
                     .ApplyQuery(new ProductGetByTitleQuery
                                 {
                                     Title = title
                                 })
                     .FirstOrDefault();
        }

        private Product? GetByTitleIncludeDeleted(string title)
        {
            using var db = _dbContextFactory.CreateDbContext();

            return db.Set<Product>()
                     .AsNoTracking()
                     .Include(x => x.StockAvailabilities)
                     .Include(x => x.Sells)
                     .FirstOrDefault(x => x.Title == title);
        }
    }
}