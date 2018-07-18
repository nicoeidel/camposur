using Camposur.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Camposur.DataAccess.Repositories.Interfaces
{
    public interface ICollectionRepository<T> where T : BaseEntity
    {
        IEnumerable<T> List(Expression<Func<T, bool>> filter = null, bool? track = null);
        int Add(T entity);
        IEnumerable<T> AddRange(ICollection<T> entities);
        void Delete(int id, bool soft = true);
        void Update(T entity);
        void UpdateEntity(T entity, T modifiedEntity);
        T FindById(int Id, bool? track = null);
        void SaveChanges();
    }
}