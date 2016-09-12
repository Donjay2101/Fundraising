using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundRaising.Models
{
    public interface IRepository<out T>
    {
        IOrderedQueryable<T> GetAll();
        T GetById(object id);
    }
}
