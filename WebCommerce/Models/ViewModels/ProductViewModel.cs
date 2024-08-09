using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace WebCommerce.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        [AllowNull]
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
