using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Linq.Expressions;
using System.Data.Entity;

using st1001.data;

namespace st1001.website.Models
{
    public class Repository<T> where T : class
    {
        protected st1001Entities context = null;

        public Repository()
        {
            this.context = new st1001Entities();
        }

        public virtual IEnumerable<T> FindAll(string include = null)
        {
            if (!string.IsNullOrEmpty(include))
            {
                return context.Set<T>().Include(include);
            }

            return context.Set<T>();
        }

        public virtual T FindById(params object[] values)
        {
            return context.Set<T>().Find(values);
        }

        public virtual void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public virtual void SetValues(T entity1, T entity2)
        {
            context.Entry(entity1).CurrentValues.SetValues(entity2);
            context.SaveChanges();
        }

        public virtual void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public virtual void Remove(params object[] values)
        {
            T entity = context.Set<T>().Find(values);

            if (entity != null)
            {
                context.Set<T>().Remove(entity);
            }
        }

        public IEnumerable<T> Elements(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().Where(exp);
        }
    }
}