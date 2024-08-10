using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var products = _productRepository.GetAll().ToList();

            var productViewModels = products.Select(p => new ProductViewModel
            {
                Product = p,
                CategoryList = _categoryRepository.GetAll().Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList()
            }).ToList();
           

            return View(productViewModels);
        }
        public string UploadImage(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "File không được null");
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, @"img\product");
            string filePath = Path.Combine(productPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return Path.Combine(@"\img\product\" + fileName);
        }


        public IActionResult Create()
        {
            ProductViewModel model = new()
            {
                CategoryList = _categoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.CategoryId.ToString()
                }),
                Product = new Product()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(ProductViewModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    model.Product.ImageURL = UploadImage(file);
                }
                _productRepository.Add(model.Product);
                _productRepository.Save();
                return RedirectToAction("Index");
            }
            model.CategoryList = _categoryRepository.GetAll().Select(u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.CategoryId.ToString()
            }).ToList();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var product = _productRepository.Get(u => u.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _categoryRepository.GetAll().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString()
            }).ToList();

            var model = new ProductViewModel
            {
                Product = product,
                CategoryList = categories
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var productFromDb = _productRepository.Get(u => u.ProductID == model.Product.ProductID);
                if (productFromDb == null)
                {
                    return NotFound();
                }

                if (file != null)
                {
                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(productFromDb.ImageURL))
                    {
                        DeleteOldImage(productFromDb.ImageURL);
                    }

                    // Upload the new image and update the URL
                    productFromDb.ImageURL = UploadImage(file);
                }

                // Update other product details
                productFromDb.ProductName = model.Product.ProductName;
                productFromDb.Description = model.Product.Description;
                productFromDb.ProductPrice = model.Product.ProductPrice;
                productFromDb.CategoryId = model.Product.CategoryId;

                _productRepository.Update(productFromDb);
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

        public void DeleteOldImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentException("Image path cannot be null or empty", nameof(imagePath));
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(wwwRootPath, imagePath.TrimStart('\\'));

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
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
        #region Apis call
        [HttpGet]
        public IActionResult GetAllFromAPI()
        {
            var products = _productRepository.GetAll().ToList();

            var productViewModels = products.Select(p => new ProductViewModel
            {
                Product = p,
                CategoryList = _categoryRepository.GetAll().Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList()
            }).ToList();
            return Json(productViewModels);
            #endregion
        }
    }
}
