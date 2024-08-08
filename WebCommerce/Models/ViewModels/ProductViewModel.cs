using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebCommerce.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
