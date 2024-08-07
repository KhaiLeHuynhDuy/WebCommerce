using Microsoft.AspNetCore.Mvc;

using WebCommerce.Models;
using WebCommerce.Repository;
using WebCommerce.Repository.IRepository;

namespace WebCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public IActionResult Index()
        {
            List<Product> products = _productRepository.GetAll().ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _productRepository.Add(product);
            _productRepository.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null|| id == 0)
            {
                return NotFound();
            }
            Product product = _productRepository.Get(u=>u.ProductID == id);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            _productRepository.Update(product);
            _productRepository.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Product product = _productRepository.Get(u => u.ProductID == id);
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(Product product)
        {
            _productRepository.Delete(product);
            _productRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
