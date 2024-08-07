using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using WebCommerce.Models;
using WebCommerce.Repository.IRepository;

namespace WebCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            List<Product> products = _productRepository.GetAll().ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _categoryRepository.GetAll().Select(
               c => new SelectListItem
               {
                   Text = c.CategoryName,
                   Value = c.CategoryId.ToString()
               }).ToList();
            ViewBag.SelectLists = categoryList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<SelectListItem> categoryList = _categoryRepository.GetAll().Select(
                   c => new SelectListItem
                   {
                       Text = c.CategoryName,
                       Value = c.CategoryId.ToString()
                   }).ToList();
                ViewBag.SelectLists = categoryList;
                return View(product);
            }
            _productRepository.Add(product);
            _productRepository.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _productRepository.Get(u => u.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> categoryList = _categoryRepository.GetAll().Select(
               c => new SelectListItem
               {
                   Text = c.CategoryName,
                   Value = c.CategoryId.ToString()
               }).ToList();
            ViewBag.SelectLists = categoryList;

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<SelectListItem> categoryList = _categoryRepository.GetAll().Select(
                   c => new SelectListItem
                   {
                       Text = c.CategoryName,
                       Value = c.CategoryId.ToString()
                   }).ToList();
                ViewBag.SelectLists = categoryList;
                return View(product);
            }
            _productRepository.Update(product);
            _productRepository.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _productRepository.Get(u => u.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id)
        {
            Product product = _productRepository.Get(u => u.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            _productRepository.Delete(product);
            _productRepository.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Upload()
        {
           return View();
        }
    }
}
