using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    //This is a generic interface.
    //When working with Genreics we won't know what the class type will be.
    //This will be a Generic type T where T is a class
    public interface IRepository<T> where T : class
    {
        //T - Category (or any other model that we want to perform the current operation)
        IEnumerable<T> GetAll(string? includeProperties = null);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
