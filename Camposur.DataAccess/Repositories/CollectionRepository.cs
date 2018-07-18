using Camposur.DataAccess.Repositories.Interfaces;
using Camposur.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace Camposur.DataAccess.Repositories
{
    public class CollectionRepository<T> : ICollectionRepository<T> where T : BaseEntity
    {
        private DbContext context;
        private DbSet<T> dbset;

        public CollectionRepository(CamposurContext ctx)
        {
            context = ctx;
            dbset = ctx.Set<T>();
        }

        public IEnumerable<T> List(Expression<Func<T, bool>> filter = null, bool? track = null)
        {
            IQueryable<T> ret;
            if (track == true)
                ret = dbset;
            else
                ret = dbset.AsNoTracking();
            return filter == null ? ret : ret.Where(filter);
        }

        public int Add(T entity)
        {
            var e = dbset.Add(entity);
            SaveChanges();
            return e.Id;
        }

        public IEnumerable<T> AddRange(ICollection<T> entities)
        {
            var e = dbset.AddRange(entities);
            SaveChanges();
            return e;
        }

        private void Delete(T entity)
        {
            // If entity is detached, fetch atached entity from dbset.
            var e = context.Entry(entity).State == EntityState.Detached ? FindById(entity.Id, true) : entity;
            dbset.Remove(e);
            SaveChanges();
        }

        public void Delete(int id, bool soft = true)
        {
            var entity = FindById(id, true);
            Delete(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            // Fetch attached entities which id is contained into entities to delete ids.
            dbset.RemoveRange(List(null, true).Where(e => entities.Select(c => c.Id).Contains(e.Id)));
            SaveChanges();
        }

        public T FindById(int Id, bool? track = null)
        {
            if (track == true)
                return dbset.FirstOrDefault(t => t.Id == Id);
            else
                return dbset.AsNoTracking().FirstOrDefault(t => t.Id == Id);
        }

        public void Update(T entity)
        {

            var oldEntity = FindById(entity.Id);
            if (oldEntity != null)
            {
                entity.CreatedDate = oldEntity.CreatedDate;
                entity.CreatedByName = oldEntity.CreatedByName;
                entity.CreatedById = oldEntity.CreatedById;
            }
            dbset.AddOrUpdate(entity);
            SaveChanges();
        }

        public void UpdateEntity(T entity, T modifiedEntity)
        {
            context.Entry(entity).CurrentValues.SetValues(modifiedEntity);
            context.Entry(entity).State = EntityState.Modified;

            Update(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
