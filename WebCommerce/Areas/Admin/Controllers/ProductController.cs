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
        public string UploadImage(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "File không được null");
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, @"img\product");

            // Ensure directory exists
            if (!Directory.Exists(productPath))
            {
                Directory.CreateDirectory(productPath);
            }

            string filePath = Path.Combine(productPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            // Return the relative path to the image
            return Path.Combine(@"img\product", fileName);
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
        public IActionResult Create(ProductViewModel model, IFormFile? file)
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

            // Cập nhật lại danh sách danh mục nếu model không hợp lệ
            model.CategoryList = _categoryRepository.GetAll().Select(u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.CategoryId.ToString()
            }).ToList();

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            // Lấy thông tin sản phẩm từ cơ sở dữ liệu
            var product = _productRepository.Get(u => u.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            // Lấy danh sách các danh mục và chuẩn bị cho View
            var categories = _categoryRepository.GetAll().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString()
            }).ToList();

            // Khởi tạo ProductViewModel với sản phẩm và danh sách danh mục
            var model = new ProductViewModel
            {
                Product = product,
                CategoryList = categories
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    productViewModel.Product.ImageURL = UploadImage(file);
                }

                _productRepository.Update(productViewModel.Product);
                _productRepository.Save();
                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ, cập nhật lại danh sách các danh mục cho View
            productViewModel.CategoryList = _categoryRepository.GetAll().Select(u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.CategoryId.ToString()
            }).ToList();

            return View(productViewModel);
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
    }
}
