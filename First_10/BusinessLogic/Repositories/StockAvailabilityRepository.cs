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
    public class StockAvailabilityRepository : IRepository<StockAvailability>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public StockAvailabilityRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory,
                                           IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public StockAvailability? GetById(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();

            return db.Set<StockAvailability>()
                     .AsNoTracking()
                     .Include(x => x.Product)
                     .FirstOrDefault(x => x.Id == id);
        }

        public ICollection<StockAvailability> GetAll()
        {
            using var db = _dbContextFactory.CreateDbContext();

            var products =
                db.Set<StockAvailability>()
                  .AsNoTracking()
                  .ApplyQuery(new StockAvailabilityGetAllQuery())
                  .ToList();

            return products;
        }

        public void Update(StockAvailability model)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var dbEntity = db.Set<StockAvailability>()
                             .First(x => x.Id == model.Id);

            _mapper.Map(model, dbEntity);

            db.SaveChanges();
        }

        public void Add(StockAvailability model)
        {
            model.Id = 0;

            using var db = _dbContextFactory.CreateDbContext();

            db.Set<StockAvailability>()
              .Add(model);

            db.SaveChanges();
        }

        public void Remove(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var dbEntity = db.Set<StockAvailability>()
                             .Find(id);

            db.Set<StockAvailability>()
              .Remove(dbEntity);

            db.SaveChanges();
        }
    }
}