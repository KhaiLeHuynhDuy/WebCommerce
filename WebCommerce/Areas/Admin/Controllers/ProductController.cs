using Microsoft.AspNetCore.Mvc;

namespace WebCommerce.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
