using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DataAccessLayer.Abstractions.Repos
{
    public interface IRepository<T> where T : class
    {
        public void Add(T entity);
        public T GetById(int id);
        public IEnumerable<T> GetAll();
        public T Find(Expression<Func<T, bool>> expression);        
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> expression);
        public bool Exists(Expression<Func<T, bool>> expression);
        public void Remove(T entity);
    }
}
