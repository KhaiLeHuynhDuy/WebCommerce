using WebCommerce.DataContext;
using WebCommerce.Models;
using WebCommerce.Repository.IRepository;

namespace WebCommerce.Repository
{
    public class CategoryRepository:Repository<Category>,ICategoryRepository
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            _context    = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
