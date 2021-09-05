using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatabaseLabes.SharedKernel.DataAccess;
using DatabaseLabes.SharedKernel.Service;
using DatabaseLabes.SharedKernel.Shared;
using First_10.DataAccess.Models;
using First_10.Queries;
using Microsoft.EntityFrameworkCore;

namespace First_10.BusinessLogic.Repositories
{
    public class SellRepository : IRepository<Sell>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public SellRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory,
                              IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Sell? GetById(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();

            return db.Set<Sell>()
                     .AsNoTracking()
                     .Include(x => x.Product)
                     .FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Sell> GetAll()
        {
            using var db = _dbContextFactory.CreateDbContext();

            var products =
                db.Set<Sell>()
                  .AsNoTracking()
                  .ApplyQuery(new SellGetAllQuery())
                  .ToList();

            return products;
        }

        public void Update(Sell model)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var dbEntity = db.Set<Sell>()
                             .First(x => x.Id == model.Id);

            _mapper.Map(model, dbEntity);

            db.SaveChanges();
        }

        public void Add(Sell model)
        {
            model.Id = 0;

            using var db = _dbContextFactory.CreateDbContext();

            db.Set<Sell>()
              .Add(model);

            db.SaveChanges();
        }

        public void Remove(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var dbEntity = db.Set<Sell>()
                             .Find(id);

            db.Set<Sell>()
              .Remove(dbEntity);

            db.SaveChanges();
        }
    }
}