using System.Collections.Generic;

namespace DatabaseLabes.SharedKernel.Service
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity model);

        void Update(TEntity model);

        void Remove(int id);

        TEntity? GetById(int id);

        ICollection<TEntity> GetAll();
    }
}