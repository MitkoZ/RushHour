using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        private RushHourContext Context;

        public BaseRepository(RushHourContext context)
        {
            this.Context = context;
        }

        public List<T> GetAll(Func<T, bool> filter = null)
        {
            if (filter != null)
            {
                return Context.Set<T>().Where(filter).ToList();
            }

            return Context.Set<T>().ToList();
        }

        public void Create(T item)
        {
            Context.Set<T>().Add(item);
        }

        public void Update(T item, Func<T, bool> findByPredicate)
        {
            var local = Context.Set<T>()
                         .Local
                         .FirstOrDefault(findByPredicate);
            Context.Entry(item).State = EntityState.Modified;
        }

        public void DeleteById(int id)
        {
            T dbItem = Context.Set<T>().Find(id);
            if (dbItem != null)
            {
                Context.Set<T>().Remove(dbItem);
            }
        }

        public void Save(T item)
        {
            if (item.Id == 0)
            {
                Create(item);
            }
            else
            {
                Update(item, x => x.Id == item.Id);
            }
        }

    }
}
