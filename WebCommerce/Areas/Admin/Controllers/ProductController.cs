using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebCommerce.Models;
using WebCommerce.Models.ViewModels;
using WebCommerce.Repository.IRepository;

namespace WebCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _productRepository.GetAll().ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ProductViewModel model = new ProductViewModel
            {
                Product = new Product(),
                CategoryList = _categoryRepository.GetAll().Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryId.ToString()
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    model.Product.ImageURL = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }

                _productRepository.Add(model.Product);
                _productRepository.Save();
                return RedirectToAction("Index");
            }
            model.CategoryList = _categoryRepository.GetAll().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString()
            }).ToList();
            return View(model);
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

            ProductViewModel model = new ProductViewModel
            {
                Product = product,
                CategoryList = _categoryRepository.GetAll().Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryId.ToString()
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    model.Product.ImageURL = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }

                _productRepository.Update(model.Product);
                _productRepository.Save();
                return RedirectToAction("Index");
            }
            model.CategoryList = _categoryRepository.GetAll().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString()
            }).ToList();
            return View(model);
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
