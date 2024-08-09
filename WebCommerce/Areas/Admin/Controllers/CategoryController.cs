using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebCommerce.Models;

using WebCommerce.Repository.IRepository;

namespace WebCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            List<Category> categories = _categoryRepository.GetAll().ToList();
           
            return View(categories);
        }
        public IActionResult Create()
        {
            return View(new Category()); // Ensure a new Category object is passed
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _categoryRepository.Add(category);
            _categoryRepository.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category category = _categoryRepository.Get(u => u.CategoryId == id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _categoryRepository.Update(category);
            _categoryRepository.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category category = _categoryRepository.Get(u => u.CategoryId == id);
            return View(category);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(Category category)
        {
            _categoryRepository.Delete(category);
            _categoryRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
