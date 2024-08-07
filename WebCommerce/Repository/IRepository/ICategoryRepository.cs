using WebCommerce.Models;

namespace WebCommerce.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Save();
    }
}
