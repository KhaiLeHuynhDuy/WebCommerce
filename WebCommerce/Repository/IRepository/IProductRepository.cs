using WebCommerce.Models;

namespace WebCommerce.Repository.IRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        public void Save();
    }
}
