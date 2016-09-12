using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundRaising.Models
{
    public abstract class SqlRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> EfDbSet;

        protected SqlRepository(DbContext context)
        {
            EfDbSet = context.Set<T>();
        }

        #region IRepository<T> Members

        public virtual IOrderedQueryable<T> GetAll()
        {
            return EfDbSet;
        }

        public abstract T GetById(object id);

        #endregion
    }
}
