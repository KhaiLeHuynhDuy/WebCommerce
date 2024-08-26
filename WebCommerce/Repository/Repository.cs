
using Microsoft.EntityFrameworkCore;
using WebCommerce.DataContext;
using WebCommerce.Repository.IRepository;

namespace WebCommerce.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void DeleteAll(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public T Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> values = dbSet;
            values = values.Where(predicate);
            return values.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> values = dbSet;
            return values.ToList();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}