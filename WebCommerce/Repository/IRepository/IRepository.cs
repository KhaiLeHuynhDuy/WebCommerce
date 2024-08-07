using System.Linq.Expressions;

namespace WebCommerce.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> predicate);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public void DeleteAll(IEnumerable<T>entity);
    }
}
